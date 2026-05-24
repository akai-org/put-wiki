using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Application.Auth;
using Application.Core;
using Application.DTOs;

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

    public async Task<Result<string>> GetLoginUrlAsync(CancellationToken cancellationToken = default)
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
        var response = await SendOAuthGetAsync(requestUrl, authHeader, cancellationToken);
        if (!response.IsSuccess || response.Value is null)
        {
            return Result.Failure<string>(response.Error ?? "USOS request token request failed.", response.Code);
        }

        var tokenPair = TryParseOAuthTokenPair(response.Value);
        if (!tokenPair.IsSuccess)
        {
            return Result.Failure<string>(tokenPair.Error ?? "USOS request token response was invalid.", tokenPair.Code);
        }

        CacheRequestTokenSecret(tokenPair.Value.Token, tokenPair.Value.Secret);

        var authorizeUrl = $"{CombineUrl(_settings.BaseUrl, AuthorizeEndpoint)}?oauth_token={tokenPair.Value.Token}";
        return Result.Success(authorizeUrl);
    }

    public async Task<Result<UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(oauthToken) || string.IsNullOrWhiteSpace(oauthVerifier))
        {
            return Result.Failure<UsosUserDto>("Missing OAuth parameters.", 400);
        }

        if (!cache.TryGetValue(GetTokenSecretCacheKey(oauthToken), out string? oauthTokenSecret) || oauthTokenSecret == null)
        {
            LogUnknownOrExpiredRequestToken();
            return Result.Failure<UsosUserDto>("Invalid or expired OAuth token.", 401);
        }

        var accessToken = await ExchangeAccessTokenAsync(oauthToken, oauthVerifier, oauthTokenSecret, cancellationToken);
        if (!accessToken.IsSuccess)
        {
            return Result.Failure<UsosUserDto>(accessToken.Error ?? "Failed to exchange access token.", accessToken.Code);
        }

        var usosUserResult = await FetchUserAsync(accessToken.Value.Token, accessToken.Value.Secret, cancellationToken);
        if (!usosUserResult.IsSuccess || usosUserResult.Value is null)
        {
            return Result.Failure<UsosUserDto>(usosUserResult.Error ?? "Failed to fetch user.", usosUserResult.Code);
        }

        return Result.Success(usosUserResult.Value);
    }

    private async Task<Result<OAuthTokenPair>> ExchangeAccessTokenAsync(
        string requestToken,
        string oauthVerifier,
        string requestTokenSecret,
        CancellationToken cancellationToken
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

        var response = await SendOAuthGetAsync(accessTokenUrl, authHeader, cancellationToken);
        if (!response.IsSuccess || response.Value is null)
        {
            return Result.Failure<OAuthTokenPair>(response.Error ?? "USOS access token request failed.", response.Code);
        }

        return TryParseOAuthTokenPair(response.Value);
    }

    private async Task<Result<UsosUserDto>> FetchUserAsync(
        string accessToken,
        string accessTokenSecret,
        CancellationToken cancellationToken
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
        var response = await SendOAuthGetAsync(requestUrl, authHeader, cancellationToken);
        if (!response.IsSuccess || response.Value is null)
        {
            return Result.Failure<UsosUserDto>(response.Error ?? "USOS user request failed.", response.Code);
        }

        UsosUserResponse? deserialized;
        try
        {
            deserialized = JsonSerializer.Deserialize<UsosUserResponse>(response.Value, JsonOptions);
        }
        catch (JsonException ex)
        {
            LogUserResponseDeserializationFailed(ex);
            return Result.Failure<UsosUserDto>("Invalid USOS user response.", 502);
        }

        if (deserialized is null)
        {
            return Result.Failure<UsosUserDto>("Invalid USOS user response.", 502);
        }

        var user = new UsosUserDto(
            Id: deserialized.Id ?? string.Empty
        );

        return Result.Success(user);
    }

    private async Task<Result<string>> SendOAuthGetAsync(
        string requestUrl,
        string authorizationHeader,
        CancellationToken cancellationToken
    )
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", authorizationHeader);

            using var response = await httpClient.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var bodyPreview = body.Length <= 256 ? body : body[..256];
                LogUsosHttpRequestFailed(requestUrl, (int)response.StatusCode, bodyPreview);

                return Result.Failure<string>("USOS request failed.", (int)response.StatusCode);
            }

            return Result.Success(body);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            LogUsosHttpRequestException(requestUrl, ex);
            return Result.Failure<string>("USOS request failed.", 502);
        }
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
            return Result.Failure<OAuthTokenPair>("Empty response.", 502);
        }

        var queryParams = HttpUtility.ParseQueryString(responseBody);
        var token = queryParams["oauth_token"];
        var secret = queryParams["oauth_token_secret"];

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(secret))
        {
            return Result.Failure<OAuthTokenPair>("Missing oauth_token or oauth_token_secret.", 502);
        }

        return Result.Success(new OAuthTokenPair(token, secret));
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