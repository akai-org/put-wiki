using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Errors;

using FluentResults;

using Infrastructure.Auth.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public partial class JwtService(
    IOptions<JwtSettings> jwtSettings,
    TimeProvider timeProvider,
    ILogger<JwtService> logger) : IJwtService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public Task<Result<string>> GenerateAccessTokenAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            var signingKey = JwtSecurityKeyFactory.CreateSigningKey(_jwtSettings.Secret);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var now = timeProvider.GetUtcNow();
            var expiration = now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims:
                [
                    new(System.Security.Claims.ClaimTypes.NameIdentifier, userId.ToString()),
                    new("sub", userId.ToString()),
                ],
                notBefore: now.UtcDateTime,
                expires: expiration.UtcDateTime,
                signingCredentials: credentials);

            var tokenString = _tokenHandler.WriteToken(token);

            LogTokenGenerated(userId);
            return Task.FromResult(Result.Ok(tokenString));
        }
        catch (Exception ex)
        {
            LogTokenGenerationFailed(ex);
            return Task.FromResult<Result<string>>(
                Result.Fail(new UnauthorizedError("Failed to generate JWT token.")));
        }
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "JWT token generated for user {userId}")]
    partial void LogTokenGenerated(Guid userId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to generate JWT token")]
    partial void LogTokenGenerationFailed(Exception ex);
}