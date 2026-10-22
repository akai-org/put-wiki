using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Application.Features.Users.Commands.ProvisionUser;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public sealed class JwtUserSessionService(
    IConfiguration configuration,
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
            ExpiresAt = timeProvider.GetUtcNow().AddDays(7).DateTime,
            CreatedAt = timeProvider.GetUtcNow().DateTime
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
            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = timeProvider.GetUtcNow().AddMinutes(30).DateTime,
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}