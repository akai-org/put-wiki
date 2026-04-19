using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Application.DTOs;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Linq;

namespace Infrastructure.Auth;

public class UsosOAuthService
{
    private readonly HttpClient _httpClient;
    private readonly UsosOAuthSettings _settings;
    private readonly IMemoryCache _cache;

    public UsosOAuthService(HttpClient httpClient, IOptions<UsosOAuthSettings> settings, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _cache = cache;
    }

    public async Task<string> GetLoginUrlAsync()
    {
        var requestTokenUrl = $"{_settings.BaseUrl}services/oauth/request_token";
        var parameters = new Dictionary<string, string>
        {
            { "oauth_callback", _settings.CallbackUrl },
            { "scopes", _settings.Scopes }
        };

        var authHeader = GenerateOAuthHeader("GET", requestTokenUrl, parameters, _settings.ConsumerSecret, null);

        var requestUrl = $"{requestTokenUrl}?scopes={OAuthUrlEncode(_settings.Scopes)}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Add("Authorization", authHeader);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"USOS Request Token Error ({response.StatusCode}): {errorBody}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var queryParams = HttpUtility.ParseQueryString(responseContent);

        var oauthToken = queryParams["oauth_token"]!;
        var oauthTokenSecret = queryParams["oauth_token_secret"]!;

        _cache.Set($"UsosTokenSecret_{oauthToken}", oauthTokenSecret, TimeSpan.FromMinutes(10));

        return $"{_settings.BaseUrl}services/oauth/authorize?oauth_token={oauthToken}";
    }

    public async Task<UsosUserDto> HandleCallbackAndGetUserAsync(string oauthToken, string oauthVerifier)
    {
        if (!_cache.TryGetValue($"UsosTokenSecret_{oauthToken}", out string? oauthTokenSecret) || oauthTokenSecret == null)
        {
            throw new UnauthorizedAccessException("Invalid or expired OAuth token.");
        }

        var accessTokenUrl = $"{_settings.BaseUrl}services/oauth/access_token";
        var parameters = new Dictionary<string, string>
        {
            { "oauth_token", oauthToken },
            { "oauth_verifier", oauthVerifier }
        };

        var authHeader = GenerateOAuthHeader("GET", accessTokenUrl, parameters, _settings.ConsumerSecret, oauthTokenSecret);

        var request = new HttpRequestMessage(HttpMethod.Get, accessTokenUrl);
        request.Headers.Add("Authorization", authHeader);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"USOS Access Token Error ({response.StatusCode}): {errorBody}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var queryParams = HttpUtility.ParseQueryString(responseContent);

        var finalAccessToken = queryParams["oauth_token"]!;
        var finalAccessTokenSecret = queryParams["oauth_token_secret"]!;

        var userUrl = $"{_settings.BaseUrl}services/users/user";
        var userParams = new Dictionary<string, string>
        {
            { "fields", "id|first_name|last_name|student_number|email" }
        };

        var finalAuthHeader = GenerateOAuthHeader("GET", userUrl, userParams, _settings.ConsumerSecret, finalAccessTokenSecret, finalAccessToken);

        var finalRequestUrl = $"{userUrl}?fields={OAuthUrlEncode("id|first_name|last_name|student_number|email")}";
        var userRequest = new HttpRequestMessage(HttpMethod.Get, finalRequestUrl);
        userRequest.Headers.Add("Authorization", finalAuthHeader);

        var userResponse = await _httpClient.SendAsync(userRequest);
        if (!userResponse.IsSuccessStatusCode)
        {
            var errorBody = await userResponse.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"USOS User Data Error ({userResponse.StatusCode}): {errorBody}");
        }

        var userJson = await userResponse.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(userJson);

        var user = new UsosUserDto
        {
            Id = doc.RootElement.TryGetProperty("id", out var id) ? id.GetString() ?? string.Empty : string.Empty,
            FirstName = doc.RootElement.TryGetProperty("first_name", out var fname) ? fname.GetString() ?? string.Empty : string.Empty,
            LastName = doc.RootElement.TryGetProperty("last_name", out var lname) ? lname.GetString() ?? string.Empty : string.Empty,
            StudentNumber = doc.RootElement.TryGetProperty("student_number", out var sn) ? sn.GetString() ?? string.Empty : string.Empty,
            Email = doc.RootElement.TryGetProperty("email", out var e) ? e.GetString() ?? string.Empty : string.Empty
        };

        return user;
    }

    private static string OAuthUrlEncode(string value)
    {
        var unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        var result = new StringBuilder();

        byte[] bytes = Encoding.UTF8.GetBytes(value);
        foreach (byte b in bytes)
        {
            char symbol = (char)b;
            if (unreservedChars.Contains(symbol))
            {
                result.Append(symbol);
            }
            else
            {
                result.Append('%' + String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:X2}", (int)b));
            }
        }

        return result.ToString();
    }

    private string GenerateOAuthHeader(string httpMethod, string url, Dictionary<string, string> extraParams, string consumerSecret, string? tokenSecret, string? token = null)
    {
        var nonce = Guid.NewGuid().ToString("N");
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(System.Globalization.CultureInfo.InvariantCulture);

        var oauthParams = new Dictionary<string, string>
        {
            { "oauth_consumer_key", _settings.ConsumerKey },
            { "oauth_nonce", nonce },
            { "oauth_signature_method", "HMAC-SHA1" },
            { "oauth_timestamp", timestamp },
            { "oauth_version", "1.0" }
        };

        if (!string.IsNullOrEmpty(token))
        {
            oauthParams.Add("oauth_token", token);
        }

        foreach (var kvp in extraParams)
        {
            oauthParams[kvp.Key] = kvp.Value;
        }

        var sortedParams = oauthParams.OrderBy(p => p.Key).ThenBy(p => p.Value);
        var parameterString = string.Join("&", sortedParams.Select(p => $"{OAuthUrlEncode(p.Key)}={OAuthUrlEncode(p.Value)}"));

        var signatureBaseString = $"{httpMethod.ToUpperInvariant()}&{OAuthUrlEncode(url)}&{OAuthUrlEncode(parameterString)}";

        var signingKey = $"{OAuthUrlEncode(consumerSecret)}&{(tokenSecret != null ? OAuthUrlEncode(tokenSecret) : "")}";

#pragma warning disable CA5350 // Do Not Use Weak Cryptographic Algorithms
        using var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(signingKey));
#pragma warning restore CA5350 // Do Not Use Weak Cryptographic Algorithms
        var signatureBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString));
        var signature = Convert.ToBase64String(signatureBytes);

        oauthParams.Add("oauth_signature", signature);

        var headerParams = oauthParams.Where(p => p.Key.StartsWith("oauth_", StringComparison.Ordinal))
                                      .OrderBy(p => p.Key)
                                      .Select(p => $"{OAuthUrlEncode(p.Key)}=\"{OAuthUrlEncode(p.Value)}\"");

        return $"OAuth {string.Join(", ", headerParams)}";
    }
}