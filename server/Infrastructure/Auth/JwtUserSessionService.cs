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
        var accessToken = GenerateAccessToken(userId);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = timeProvider.GetUtcNow().Add(settings.Value.RefreshTokenLifetime),
            CreatedAt = timeProvider.GetUtcNow()
        };

        dbContext.RefreshTokens.Add(refreshToken);

        return new JwtTokenPair(accessToken, refreshToken.Token);
    }

    /**
     * NOTE: after calling this method use IUnitOfWork to persist changes
     */
    public async Task<JwtTokenPair?> RefreshSession(string refreshToken)
    {
        var storedTokenObject = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

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

        storedTokenObject.IsRevoked = true;

        return CreateSession(storedTokenObject.UserId);
    }

    private string GenerateAccessToken(Guid userId)
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
            Expires = timeProvider.GetUtcNow().Add(settings.Value.AccessTokenLifetime).UtcDateTime,
            SigningCredentials = credentials,
            Issuer = settings.Value.Issuer,
            Audience = settings.Value.Audience
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }

    [LoggerMessage(LogLevel.Critical, "Tried to refresh session of user with ID: {userId} with refresh token that is already revoked")]
    partial void LogRefreshTokenTheftOccured(string userId);
}