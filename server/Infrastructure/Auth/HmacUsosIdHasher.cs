using System;
using System.Security.Cryptography;
using System.Text;

using Application.Auth;

using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public class HmacUsosIdHasher(IOptions<UsosOAuthSettings> options) : IUsosIdHasher
{
    private readonly byte[] _keyBytes = Encoding.UTF8.GetBytes(options.Value.HashingKey);

    public string Hash(string rawUsosId)
    {
        using var hmac = new HMACSHA256(_keyBytes);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawUsosId));
        return Convert.ToBase64String(hashBytes);
    }
}