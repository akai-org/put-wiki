using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

using Application.Auth;
using Application.Core;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public class UsosOAuthService : IUsosOAuthService
{
    private const string RequestTokenEndpoint = "services/oauth/request_token";
    private const string AccessTokenEndpoint = "services/oauth/access_token";
    private const string AuthorizeEndpoint = "services/oauth/authorize";
    private const string UserEndpoint = "services/users/user";

    private const string UserFields = "id|first_name|last_name|student_number|email";
    private const string TokenSecretCacheKeyPrefix = "UsosTokenSecret_";

    private readonly HttpClient _httpClient;
    private readonly Application.Auth.UsosOAuthSettings _settings;
    private readonly IMemoryCache _cache;
    private readonly ILogger<UsosOAuthService> _logger;

    private static readonly Action<ILogger, Exception?> LogUnknownOrExpiredRequestToken = LoggerMessage.Define(
        LogLevel.Warning,
        new EventId(1, nameof(LogUnknownOrExpiredRequestToken)),
        "USOS callback received unknown/expired request token."
    );

    private static readonly Action<ILogger, string, int, string, Exception?> LogUsosHttpRequestFailed = LoggerMessage.Define<string, int, string>(
        LogLevel.Warning,
        new EventId(2, nameof(LogUsosHttpRequestFailed)),
        "USOS HTTP request failed. Url: {Url} Status: {StatusCode} BodyPreview: {BodyPreview}"
    );

    private static readonly Action<ILogger, Exception?> LogUserResponseDeserializationFailed = LoggerMessage.Define(
        LogLevel.Error,
        new EventId(3, nameof(LogUserResponseDeserializationFailed)),
        "Failed to deserialize USOS user response."
    );

    private static readonly Action<ILogger, string, Exception?> LogUsosHttpRequestException = LoggerMessage.Define<string>(
        LogLevel.Error,
        new EventId(4, nameof(LogUsosHttpRequestException)),
        "USOS HTTP request threw an exception. Url: {Url}"
    );

    private static readonly Action<ILogger, string, Exception?> LogUsosSettingsInvalid = LoggerMessage.Define<string>(
        LogLevel.Error,
        new EventId(5, nameof(LogUsosSettingsInvalid)),
        "USOS OAuth settings are invalid: {Reason}"
    );

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public UsosOAuthService(
        HttpClient httpClient,
        IOptions<Application.Auth.UsosOAuthSettings> settings,
        IMemoryCache cache,
        ILogger<UsosOAuthService> logger
    )
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<string>> GetLoginUrlAsync(System.Threading.CancellationToken cancellationToken = default)
    {
        var settingsValidation = ValidateSettingsForLogin();
        if (!settingsValidation.IsSuccess)
        {
            LogUsosSettingsInvalid(_logger, settingsValidation.Error ?? "Invalid settings.", null);
            return Result.Failure<string>(settingsValidation.Error ?? "Invalid USOS OAuth settings.", settingsValidation.Code);
        }

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

    public async Task<Result<IUsosOAuthService.UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        System.Threading.CancellationToken cancellationToken = default
    )
    {
        var settingsValidation = ValidateSettingsForCallback();
        if (!settingsValidation.IsSuccess)
        {
            LogUsosSettingsInvalid(_logger, settingsValidation.Error ?? "Invalid settings.", null);
            return Result.Failure<IUsosOAuthService.UsosUserDto>(settingsValidation.Error ?? "Invalid USOS OAuth settings.", settingsValidation.Code);
        }

        if (string.IsNullOrWhiteSpace(oauthToken) || string.IsNullOrWhiteSpace(oauthVerifier))
        {
            return Result.Failure<IUsosOAuthService.UsosUserDto>("Missing OAuth parameters.", 400);
        }

        if (!_cache.TryGetValue(GetTokenSecretCacheKey(oauthToken), out string? oauthTokenSecret) || oauthTokenSecret == null)
        {
            LogUnknownOrExpiredRequestToken(_logger, null);
            return Result.Failure<IUsosOAuthService.UsosUserDto>("Invalid or expired OAuth token.", 401);
        }

        var accessToken = await ExchangeAccessTokenAsync(oauthToken, oauthVerifier, oauthTokenSecret, cancellationToken);
        if (!accessToken.IsSuccess || accessToken.Value.Token is null || accessToken.Value.Secret is null)
        {
            return Result.Failure<IUsosOAuthService.UsosUserDto>(accessToken.Error ?? "Failed to exchange access token.", accessToken.Code);
        }

        var user = await FetchUserAsync(accessToken.Value.Token, accessToken.Value.Secret, cancellationToken);
        if (!user.IsSuccess || user.Value is null)
        {
            return Result.Failure<IUsosOAuthService.UsosUserDto>(user.Error ?? "Failed to fetch user.", user.Code);
        }

        return Result.Success(user.Value);
    }

    private async Task<Result<OAuthTokenPair>> ExchangeAccessTokenAsync(
        string requestToken,
        string oauthVerifier,
        string requestTokenSecret,
        System.Threading.CancellationToken cancellationToken
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

    private async Task<Result<IUsosOAuthService.UsosUserDto>> FetchUserAsync(
        string accessToken,
        string accessTokenSecret,
        System.Threading.CancellationToken cancellationToken
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
            return Result.Failure<IUsosOAuthService.UsosUserDto>(response.Error ?? "USOS user request failed.", response.Code);
        }

        UsosUserResponse? deserialized;
        try
        {
            deserialized = JsonSerializer.Deserialize<UsosUserResponse>(response.Value, JsonOptions);
        }
        catch (JsonException ex)
        {
            LogUserResponseDeserializationFailed(_logger, ex);
            return Result.Failure<IUsosOAuthService.UsosUserDto>("Invalid USOS user response.", 502);
        }

        if (deserialized is null)
        {
            return Result.Failure<IUsosOAuthService.UsosUserDto>("Invalid USOS user response.", 502);
        }

        var user = new IUsosOAuthService.UsosUserDto(
            Id: deserialized.Id ?? string.Empty,
            FirstName: deserialized.FirstName ?? string.Empty,
            LastName: deserialized.LastName ?? string.Empty,
            StudentNumber: deserialized.StudentNumber ?? string.Empty,
            Email: deserialized.Email ?? string.Empty
        );

        return Result.Success(user);
    }

    private async Task<Result<string>> SendOAuthGetAsync(
        string requestUrl,
        string authorizationHeader,
        System.Threading.CancellationToken cancellationToken
    )
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", authorizationHeader);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var bodyPreview = body.Length <= 256 ? body : body[..256];
                LogUsosHttpRequestFailed(_logger, requestUrl, (int)response.StatusCode, bodyPreview, null);

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
            LogUsosHttpRequestException(_logger, requestUrl, ex);
            return Result.Failure<string>("USOS request failed.", 502);
        }
    }

    private void CacheRequestTokenSecret(string token, string secret)
    {
        _cache.Set(GetTokenSecretCacheKey(token), secret, TimeSpan.FromMinutes(10));
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

    private Result<string> ValidateSettingsForLogin()
    {
        if (!TryValidateBaseUrl(out var error))
        {
            return Result.Failure<string>(error!, 500);
        }

        if (string.IsNullOrWhiteSpace(_settings.ConsumerKey) || string.IsNullOrWhiteSpace(_settings.ConsumerSecret))
        {
            return Result.Failure<string>("Missing UsosOAuth consumer key/secret.", 500);
        }

        if (string.IsNullOrWhiteSpace(_settings.CallbackUrl))
        {
            return Result.Failure<string>("Missing UsosOAuth callback URL.", 500);
        }

        if (string.IsNullOrWhiteSpace(_settings.Scopes))
        {
            return Result.Failure<string>("Missing UsosOAuth scopes.", 500);
        }

        return Result.Success("ok");
    }

    private Result<string> ValidateSettingsForCallback()
    {
        if (!TryValidateBaseUrl(out var error))
        {
            return Result.Failure<string>(error!, 500);
        }

        if (string.IsNullOrWhiteSpace(_settings.ConsumerKey) || string.IsNullOrWhiteSpace(_settings.ConsumerSecret))
        {
            return Result.Failure<string>("Missing UsosOAuth consumer key/secret.", 500);
        }

        return Result.Success("ok");
    }

    private bool TryValidateBaseUrl(out string? error)
    {
        error = null;

        if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
        {
            error = "Missing UsosOAuth base URL (set UsosOAuth__BaseUrl).";
            return false;
        }

        if (!Uri.TryCreate(_settings.BaseUrl, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            error = "Invalid UsosOAuth base URL (must be an absolute http(s) URL).";
            return false;
        }

        return true;
    }

    private readonly record struct OAuthTokenPair(string Token, string Secret);

    private sealed record UsosUserResponse(
        [property: JsonPropertyName("id")] string? Id,
        [property: JsonPropertyName("first_name")] string? FirstName,
        [property: JsonPropertyName("last_name")] string? LastName,
        [property: JsonPropertyName("student_number")] string? StudentNumber,
        [property: JsonPropertyName("email")] string? Email
    );
}