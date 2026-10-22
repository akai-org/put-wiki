using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Application.Features.Users.Commands.ProvisionUser;

using Infrastructure.Auth.Configuration;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public sealed class JwtUserSessionService(
    IOptions<JwtSettings> settings,
    AppDbContext dbContext,
    TimeProvider timeProvider
    ) : IUserSessionService
{
    public (string AccessToken, string RefreshToken) CreateSession(Guid userId)
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

        return (accessToken, refreshToken.Token);
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
}