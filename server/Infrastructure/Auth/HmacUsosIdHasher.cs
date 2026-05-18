using System;
using System.Text;

using Application.Auth;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public class HmacUsosIdHasher : IUsosIdHasher
{
    private readonly string _key;

    public HmacUsosIdHasher(IOptions<UsosOAuthSettings> options)
    {
        _key = options.Value.HashingKey;
    }

    public string Hash(string rawUsosId)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(_key));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawUsosId));
        return Convert.ToBase64String(hashBytes);
    }
}