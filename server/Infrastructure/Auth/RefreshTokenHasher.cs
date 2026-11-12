using System;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Auth;

public static class RefreshTokenHasher
{
    public static string CreateHash(string rawToken)
    {
        var bytes = Encoding.UTF8.GetBytes(rawToken);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}