using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Application.Auth;
using Application.DTOs;
using Application.Errors;

using FluentResults;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public partial class UsosOAuthService(
    IUsosHttpClient usosHttpClient,
    IOptions<UsosOAuthSettings> settings,
    IMemoryCache cache,
    ILogger<UsosOAuthService> logger
    ) : IUsosOAuthService
{
    private const string RequestTokenEndpoint = "services/oauth/request_token";
    private const string AccessTokenEndpoint = "services/oauth/access_token";
    private const string AuthorizeEndpoint = "services/oauth/authorize";
    private const string UserEndpoint = "services/users/user";

    private const string UserFields = "id";
    private const string TokenSecretCacheKeyPrefix = "UsosTokenSecret_";

    private readonly UsosOAuthSettings _settings = settings.Value;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Result<string>> GetLoginUrlAsync(CancellationToken ct = default)
    {
        var authParams = new Dictionary<string, string> { ["oauth_callback"] = _settings.CallbackUrl };
        var queryParams = new Dictionary<string, string> { ["scopes"] = _settings.Scopes };

        var oAuthResponse = await usosHttpClient.GetAsync(
            endpoint: RequestTokenEndpoint,
            queryParameters: queryParams,
            additionalAuthParameters: authParams,
            ct: ct);

        if (oAuthResponse.IsFailed)
            return oAuthResponse.ToResult();

        var tokenPair = TryParseOAuthTokenPair(oAuthResponse.Value);
        if (tokenPair.IsFailed)
            return tokenPair.ToResult();

        CacheRequestTokenSecret(tokenPair.Value.Token, tokenPair.Value.Secret);

        var authorizeUrl = $"{_settings.BaseUrl.TrimEnd('/')}/{AuthorizeEndpoint}?oauth_token={tokenPair.Value.Token}";
        return Result.Ok(authorizeUrl);
    }

    public async Task<Result<UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(oauthToken) || string.IsNullOrWhiteSpace(oauthVerifier))
        {
            return Result.Fail(new ValidationError("Missing OAuth parameters."));
        }

        if (!cache.TryGetValue(GetTokenSecretCacheKey(oauthToken), out string? oauthTokenSecret) || oauthTokenSecret == null)
        {
            LogUnknownOrExpiredRequestToken();
            return Result.Fail(new UnauthorizedError("Invalid or expired OAuth token."));
        }

        var accessToken = await ExchangeAccessTokenAsync(oauthToken, oauthVerifier, oauthTokenSecret, ct);
        if (accessToken.IsFailed)
            return accessToken.ToResult();

        var usosUserResult = await FetchUserAsync(accessToken.Value.Token, accessToken.Value.Secret, ct);
        if (usosUserResult.IsFailed)
            return usosUserResult.ToResult();

        return Result.Ok(usosUserResult.Value);
    }

    private async Task<Result<OAuthTokenPair>> ExchangeAccessTokenAsync(
        string requestToken,
        string oauthVerifier,
        string requestTokenSecret,
        CancellationToken ct)
    {
        var authParams = new Dictionary<string, string>
        {
            ["oauth_token"] = requestToken,
            ["oauth_verifier"] = oauthVerifier,
        };

        var oAuthResponse = await usosHttpClient.GetAsync(
            endpoint: AccessTokenEndpoint,
            additionalAuthParameters: authParams,
            tokenSecret: requestTokenSecret,
            ct: ct);

        if (oAuthResponse.IsFailed)
            return Result.Fail(new ExternalServiceError("USOS access token request failed."));

        return TryParseOAuthTokenPair(oAuthResponse.Value);
    }

    private async Task<Result<UsosUserDto>> FetchUserAsync(
        string accessToken,
        string accessTokenSecret,
        CancellationToken ct)
    {
        var queryParams = new Dictionary<string, string> { ["fields"] = UserFields };

        var oAuthResponse = await usosHttpClient.GetAsync(
            endpoint: UserEndpoint,
            queryParameters: queryParams,
            tokenSecret: accessTokenSecret,
            token: accessToken,
            ct: ct);

        if (oAuthResponse.IsFailed)
            return Result.Fail(new ExternalServiceError("USOS user request failed."));

        try
        {
            var deserializedUsosUser = JsonSerializer.Deserialize<UsosUserResponse>(oAuthResponse.Value, JsonOptions);
            if (deserializedUsosUser?.Id == null)
                return Result.Fail(new ExternalServiceError("Invalid USOS user response."));

            return Result.Ok(new UsosUserDto(deserializedUsosUser.Id));
        }
        catch (JsonException ex)
        {
            LogUserResponseDeserializationFailed(ex);
            return Result.Fail(new ExternalServiceError("Invalid USOS user response."));
        }
    }

    private void CacheRequestTokenSecret(string token, string secret) =>
        cache.Set(GetTokenSecretCacheKey(token), secret, TimeSpan.FromMinutes(10));

    private static string GetTokenSecretCacheKey(string token) => $"{TokenSecretCacheKeyPrefix}{token}";

    private static Result<OAuthTokenPair> TryParseOAuthTokenPair(string? responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody)) return Result.Fail(new ExternalServiceError("Empty response"));

        var queryParams = HttpUtility.ParseQueryString(responseBody);
        var token = queryParams["oauth_token"];
        var secret = queryParams["oauth_token_secret"];

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(secret))
            return Result.Fail(new ExternalServiceError("Missing oauth_token or oauth_token_secret."));

        return Result.Ok(new OAuthTokenPair(token, secret));
    }

    private readonly record struct OAuthTokenPair(string Token, string Secret);
    private sealed record UsosUserResponse([property: JsonPropertyName("id")] string? Id);

    [LoggerMessage(Level = LogLevel.Warning, Message = "USOS callback received unknown/expired request token.")]
    public partial void LogUnknownOrExpiredRequestToken(Exception? ex = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to deserialize USOS user response.")]
    public partial void LogUserResponseDeserializationFailed(Exception? ex);
}