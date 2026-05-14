using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Auth;

internal static class OAuth1Helper
{
    internal const string SignatureMethod = "HMAC-SHA1";
    internal const string Version = "1.0";

    internal static Dictionary<string, string> BuildOAuthParameters(string consumerKey, string nonce, string timestamp, string? token)
    {
        var parameters = new Dictionary<string, string>
        {
            ["oauth_consumer_key"] = consumerKey,
            ["oauth_nonce"] = nonce,
            ["oauth_signature_method"] = SignatureMethod,
            ["oauth_timestamp"] = timestamp,
            ["oauth_version"] = Version,
        };

        if (!string.IsNullOrWhiteSpace(token))
        {
            parameters["oauth_token"] = token;
        }

        return parameters;
    }

    internal static string CreateSignatureBaseString(string httpMethod, string url, IReadOnlyDictionary<string, string> parameters)
    {
        var sorted = parameters.OrderBy(p => p.Key, StringComparer.Ordinal).ThenBy(p => p.Value, StringComparer.Ordinal);
        var parameterString = string.Join("&", sorted.Select(p => $"{UrlEncode(p.Key)}={UrlEncode(p.Value)}"));
        return $"{httpMethod.ToUpperInvariant()}&{UrlEncode(url)}&{UrlEncode(parameterString)}";
    }

    internal static string ComputeHmacSha1Signature(string signatureBaseString, string consumerSecret, string? tokenSecret)
    {
        var signingKey = $"{UrlEncode(consumerSecret)}&{UrlEncode(tokenSecret ?? string.Empty)}";

#pragma warning disable CA5350 // Do Not Use Weak Cryptographic Algorithms
        using var hmac = new HMACSHA1(Encoding.ASCII.GetBytes(signingKey));
#pragma warning restore CA5350 // Do Not Use Weak Cryptographic Algorithms

        var signatureBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString));
        return Convert.ToBase64String(signatureBytes);
    }

    internal static string BuildAuthorizationHeader(IReadOnlyDictionary<string, string> parameters)
    {
        var headerParams = parameters
            .Where(p => p.Key.StartsWith("oauth_", StringComparison.Ordinal))
            .OrderBy(p => p.Key, StringComparer.Ordinal)
            .Select(p => $"{UrlEncode(p.Key)}=\"{UrlEncode(p.Value)}\"");

        return $"OAuth {string.Join(", ", headerParams)}";
    }

    internal static string UrlEncode(string value)
    {
        const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        var result = new StringBuilder();

        var bytes = Encoding.UTF8.GetBytes(value);
        foreach (byte b in bytes)
        {
            char symbol = (char)b;
            if (unreservedChars.Contains(symbol, StringComparison.Ordinal))
            {
                result.Append(symbol);
            }
            else
            {
                result.Append('%' + string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:X2}", (int)b));
            }
        }

        return result.ToString();
    }
}