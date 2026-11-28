using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Application.Auth;

using Infrastructure.Auth.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public sealed partial class JwtUserSessionService(
    IOptions<JwtSettings> settings,
    AppDbContext dbContext,
    TimeProvider timeProvider,
    ILogger<JwtUserSessionService> logger
    ) : IUserSessionService
{
    /**
     * NOTE: after calling this method use IUnitOfWork to persist changes
     */
    public JwtTokenPair CreateSession(Guid userId)
    {
        var now = timeProvider.GetUtcNow();
        var accessTokenExpiresAt = now.Add(settings.Value.AccessTokenLifetime);
        var refreshTokenExpiresAt = now.Add(settings.Value.RefreshTokenLifetime);

        var accessToken = GenerateAccessToken(userId, accessTokenExpiresAt);

        var rawRefreshTokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var hashedRefreshTokenString = RefreshTokenHasher.CreateHash(rawRefreshTokenString);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = hashedRefreshTokenString,
            ExpiresAt = refreshTokenExpiresAt,
            CreatedAt = now
        };

        dbContext.RefreshTokens.Add(refreshToken);

        return new JwtTokenPair(
            AccessToken: accessToken,
            AccessTokenExpiresAt: accessTokenExpiresAt,
            RefreshToken: rawRefreshTokenString,
            RefreshTokenExpiresAt: refreshTokenExpiresAt
        );
    }

    /**
     * NOTE: after calling this method use IUnitOfWork to persist changes
     */
    public async Task<JwtTokenPair?> RefreshSession(string rawRefreshTokenString)
    {
        var hashedRefreshTokenString = RefreshTokenHasher.CreateHash(rawRefreshTokenString);

        var storedTokenObject = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == hashedRefreshTokenString);

        if (storedTokenObject is null || storedTokenObject.ExpiresAt <= timeProvider.GetUtcNow())
        {
            return null;
        }

        if (storedTokenObject.IsRevoked)
        {
            // NOTE: due to high severity it runs immediate batch update skipping ef core change tracker
            await dbContext.RefreshTokens
                .Where(r => r.UserId == storedTokenObject.UserId && !r.IsRevoked)
                .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsRevoked, true));

            LogRefreshTokenTheftOccured(storedTokenObject.UserId.ToString());

            return null;
        }

        var rowsAffected = await dbContext.RefreshTokens
            .Where(r => r.Token == hashedRefreshTokenString && !r.IsRevoked)
            .ExecuteUpdateAsync(s => s
                .SetProperty(r => r.IsRevoked, true));
        if (rowsAffected == 0)
        {
            return null;
        }

        return CreateSession(storedTokenObject.UserId);
    }

    private string GenerateAccessToken(Guid userId, DateTimeOffset expiresAt)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(settings.Value.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt.UtcDateTime,
            SigningCredentials = credentials,
            Issuer = settings.Value.Issuer,
            Audience = settings.Value.Audience
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }

    [LoggerMessage(LogLevel.Warning, "Tried to refresh session of user with ID: {userId} with refresh token that is already revoked")]
    partial void LogRefreshTokenTheftOccured(string userId);
}