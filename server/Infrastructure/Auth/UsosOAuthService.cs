using System;
using System.Collections.Generic;
using System.Net.Http;
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
    HttpClient httpClient,
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
        var requestTokenUrl = CombineUrl(_settings.BaseUrl, RequestTokenEndpoint);

        var extraParameters = new Dictionary<string, string>
        {
            ["oauth_callback"] = _settings.CallbackUrl,
            ["scopes"] = _settings.Scopes,
        };

        var authHeader = GenerateOAuthHeader(
            httpMethod: "GET",
            url: requestTokenUrl,
            extraParams: extraParameters,
            consumerSecret: _settings.ConsumerSecret,
            tokenSecret: null,
            token: null
        );

        var requestUrl = $"{requestTokenUrl}?scopes={OAuth1Helper.UrlEncode(_settings.Scopes)}";
        var oAuthResponse = await SendOAuthGetAsync(requestUrl, authHeader, ct);
        if (oAuthResponse.IsFailed)
            return oAuthResponse.ToResult();

        var tokenPair = TryParseOAuthTokenPair(oAuthResponse.Value);
        if (tokenPair.IsFailed)
            return tokenPair.ToResult();

        CacheRequestTokenSecret(tokenPair.Value.Token, tokenPair.Value.Secret);

        var authorizeUrl = $"{CombineUrl(_settings.BaseUrl, AuthorizeEndpoint)}?oauth_token={tokenPair.Value.Token}";
        return Result.Ok(authorizeUrl);
    }

    public async Task<Result<UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        CancellationToken ct = default
    )
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
        CancellationToken ct
    )
    {
        var accessTokenUrl = CombineUrl(_settings.BaseUrl, AccessTokenEndpoint);

        var extraParameters = new Dictionary<string, string>
        {
            ["oauth_token"] = requestToken,
            ["oauth_verifier"] = oauthVerifier,
        };

        var authHeader = GenerateOAuthHeader(
            httpMethod: "GET",
            url: accessTokenUrl,
            extraParams: extraParameters,
            consumerSecret: _settings.ConsumerSecret,
            tokenSecret: requestTokenSecret,
            token: null
        );

        var oAuthResponse = await SendOAuthGetAsync(accessTokenUrl, authHeader, ct);
        if (oAuthResponse.IsFailed)
            return Result.Fail(new ExternalServiceError("USOS access token request failed."));

        return TryParseOAuthTokenPair(oAuthResponse.Value);
    }

    private async Task<Result<UsosUserDto>> FetchUserAsync(
        string accessToken,
        string accessTokenSecret,
        CancellationToken ct
    )
    {
        var userUrl = CombineUrl(_settings.BaseUrl, UserEndpoint);

        var extraParameters = new Dictionary<string, string>
        {
            ["fields"] = UserFields,
        };

        var authHeader = GenerateOAuthHeader(
            httpMethod: "GET",
            url: userUrl,
            extraParams: extraParameters,
            consumerSecret: _settings.ConsumerSecret,
            tokenSecret: accessTokenSecret,
            token: accessToken
        );

        var requestUrl = $"{userUrl}?fields={OAuth1Helper.UrlEncode(UserFields)}";
        var oAuthResponse = await SendOAuthGetAsync(requestUrl, authHeader, ct);
        if (oAuthResponse.IsFailed)
            return Result.Fail(new ExternalServiceError("USOS user request failed."));

        UsosUserResponse? deserializedUsosUser;
        try
        {
            deserializedUsosUser = JsonSerializer.Deserialize<UsosUserResponse>(oAuthResponse.Value, JsonOptions);
        }
        catch (JsonException ex)
        {
            LogUserResponseDeserializationFailed(ex);
            return Result.Fail(new ExternalServiceError("Invalid USOS user response."));
        }

        if (deserializedUsosUser is null)
        {
            return Result.Fail(new ExternalServiceError("Invalid USOS user response."));
        }

        var user = new UsosUserDto(
            Id: deserializedUsosUser.Id ?? string.Empty
        );

        return Result.Ok(user);
    }

    private async Task<Result<string>> SendOAuthGetAsync(
        string requestUrl,
        string authorizationHeader,
        CancellationToken ct
    )
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Add("Authorization", authorizationHeader);

        using var response = await httpClient.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            var bodyPreview = body.Length <= 256 ? body : body[..256];
            LogUsosHttpRequestFailed(requestUrl, (int)response.StatusCode, bodyPreview);

            return Result.Fail(new ExternalServiceError("USOS request failed due to external API error."));
        }

        return Result.Ok(body);
    }

    private void CacheRequestTokenSecret(string token, string secret)
    {
        cache.Set(GetTokenSecretCacheKey(token), secret, TimeSpan.FromMinutes(10));
    }

    private static string GetTokenSecretCacheKey(string token) => $"{TokenSecretCacheKeyPrefix}{token}";

    private static string CombineUrl(string baseUrl, string relative)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return relative;
        }

        if (!baseUrl.EndsWith('/'))
        {
            baseUrl += "/";
        }

        return baseUrl + relative;
    }

    private string GenerateOAuthHeader(
        string httpMethod,
        string url,
        Dictionary<string, string> extraParams,
        string consumerSecret,
        string? tokenSecret,
        string? token
    )
    {
        var nonce = Guid.NewGuid().ToString("N");
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(System.Globalization.CultureInfo.InvariantCulture);

        var oauthParameters = OAuth1Helper.BuildOAuthParameters(
            consumerKey: _settings.ConsumerKey,
            nonce: nonce,
            timestamp: timestamp,
            token: token
        );

        foreach (var kvp in extraParams)
        {
            oauthParameters[kvp.Key] = kvp.Value;
        }

        var signatureBaseString = OAuth1Helper.CreateSignatureBaseString(httpMethod, url, oauthParameters);
        var signature = OAuth1Helper.ComputeHmacSha1Signature(signatureBaseString, consumerSecret, tokenSecret);
        oauthParameters["oauth_signature"] = signature;

        return OAuth1Helper.BuildAuthorizationHeader(oauthParameters);
    }

    private static Result<OAuthTokenPair> TryParseOAuthTokenPair(string? responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return Result.Fail(new ExternalServiceError("Empty response"));
        }

        var queryParams = HttpUtility.ParseQueryString(responseBody);
        var token = queryParams["oauth_token"];
        var secret = queryParams["oauth_token_secret"];

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(secret))
        {
            return Result.Fail(new ExternalServiceError("Missing oauth_token or oauth_token_secret."));
        }

        return Result.Ok(new OAuthTokenPair(token, secret));
    }

    private readonly record struct OAuthTokenPair(string Token, string Secret);

    private sealed record UsosUserResponse(
        [property: JsonPropertyName("id")] string? Id
    );

    [LoggerMessage(Level = LogLevel.Warning, Message = "USOS callback received unknown/expired request token.")]
    public partial void LogUnknownOrExpiredRequestToken(Exception? ex = null);

    [LoggerMessage(Level = LogLevel.Warning, Message = "USOS HTTP request failed. Url: {url} Status: {statusCode} BodyPreview: {bodyPreview}")]
    public partial void LogUsosHttpRequestFailed(string url, int statusCode, string bodyPreview, Exception? ex = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to deserialize USOS user response.")]
    public partial void LogUserResponseDeserializationFailed(Exception? ex);

    [LoggerMessage(Level = LogLevel.Error, Message = "USOS HTTP request threw an exception. Url: {url}")]
    public partial void LogUsosHttpRequestException(string url, Exception? ex);
}