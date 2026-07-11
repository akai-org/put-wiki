using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Application.Errors;

using FluentResults;

using Infrastructure.Auth;
using Infrastructure.Auth.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Clients;

public partial class UsosHttpClient(
    HttpClient httpClient,
    IOptions<UsosOAuthSettings> settings,
    ILogger<UsosHttpClient> logger)
    : IUsosHttpClient
{
    private readonly UsosOAuthSettings _settings = settings.Value;

    public async Task<Result<string>> GetAsync(
        string endpoint,
        Dictionary<string, string>? queryParameters = null,
        Dictionary<string, string>? additionalAuthParameters = null,
        string? tokenSecret = null,
        string? token = null,
        CancellationToken ct = default)
    {
        var baseUrl = CombineUrl(_settings.BaseUrl, endpoint);

        var allParams = new Dictionary<string, string>();
        if (queryParameters != null)
        {
            foreach (var kvp in queryParameters)
            {
                allParams[kvp.Key] = kvp.Value;
            }
        }
        if (additionalAuthParameters != null)
        {
            foreach (var kvp in additionalAuthParameters)
            {
                allParams[kvp.Key] = kvp.Value;
            }
        }

        var authHeader = GenerateOAuthHeader(
            httpMethod: "GET",
            url: baseUrl,
            extraParams: allParams,
            consumerSecret: _settings.ConsumerSecret,
            tokenSecret: tokenSecret,
            token: token
        );

        var requestUrl = baseUrl;
        if (queryParameters != null && queryParameters.Count > 0)
        {
            var queryString = string.Join("&", queryParameters.Select(kvp => $"{kvp.Key}={OAuth1Helper.UrlEncode(kvp.Value)}"));
            requestUrl = $"{baseUrl}?{queryString}";
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Add("Authorization", authHeader);

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

    private string GenerateOAuthHeader(
        string httpMethod,
        string url,
        Dictionary<string, string> extraParams,
        string consumerSecret,
        string? tokenSecret,
        string? token)
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

    [LoggerMessage(Level = LogLevel.Warning, Message = "USOS HTTP request failed. Url: {url} Status: {statusCode} BodyPreview: {bodyPreview}")]
    public partial void LogUsosHttpRequestFailed(string url, int statusCode, string bodyPreview, Exception? ex = null);
}