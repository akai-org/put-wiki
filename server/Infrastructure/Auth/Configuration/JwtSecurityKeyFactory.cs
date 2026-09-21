using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth.Configuration;

public static class JwtSecurityKeyFactory
{
    public static SymmetricSecurityKey CreateSigningKey(string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        return new SymmetricSecurityKey(keyBytes);
    }
}