using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
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

    public Task<Result<string>> GenerateTokenAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
                return Task.FromResult<Result<string>>(
                    Result.Fail(new UnauthorizedError("JWT secret is not configured.")));

            var key = GetSecurityKey(_jwtSettings.Secret);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = timeProvider.GetUtcNow();
            var expiration = now.AddMinutes(_jwtSettings.ExpirationMinutes);

            var claims = new List<System.Security.Claims.Claim>
            {
                new(System.Security.Claims.ClaimTypes.NameIdentifier, userId.ToString()),
                new("sub", userId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
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

    public Task<Result<Guid>> ValidateTokenAsync(string token, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult<Result<Guid>>(
                    Result.Fail(new UnauthorizedError("Token is empty.")));

            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
                return Task.FromResult<Result<Guid>>(
                    Result.Fail(new UnauthorizedError("JWT secret is not configured.")));

            var key = GetSecurityKey(_jwtSettings.Secret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = !string.IsNullOrWhiteSpace(_jwtSettings.Issuer),
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = !string.IsNullOrWhiteSpace(_jwtSettings.Audience),
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            var principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? principal.FindFirst("sub");

            if (userIdClaim?.Value == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Task.FromResult<Result<Guid>>(
                    Result.Fail(new UnauthorizedError("Invalid user ID in token.")));

            LogTokenValidated(userId);
            return Task.FromResult(Result.Ok(userId));
        }
        catch (SecurityTokenExpiredException ex)
        {
            LogTokenValidationFailed(ex);
            return Task.FromResult<Result<Guid>>(
                Result.Fail(new UnauthorizedError("Token has expired.")));
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            LogTokenValidationFailed(ex);
            return Task.FromResult<Result<Guid>>(
                Result.Fail(new UnauthorizedError("Token signature is invalid.")));
        }
        catch (Exception ex)
        {
            LogTokenValidationFailed(ex);
            return Task.FromResult<Result<Guid>>(
                Result.Fail(new UnauthorizedError("Failed to validate JWT token.")));
        }
    }

    private static SymmetricSecurityKey GetSecurityKey(string secret)
    {
        var key = Encoding.UTF8.GetBytes(secret);
        if (key.Length < 32)
            key = SHA256.HashData(key);
        return new SymmetricSecurityKey(key);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "JWT token generated for user {userId}")]
    partial void LogTokenGenerated(Guid userId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to generate JWT token")]
    partial void LogTokenGenerationFailed(Exception ex);

    [LoggerMessage(Level = LogLevel.Information, Message = "JWT token validated for user {userId}")]
    partial void LogTokenValidated(Guid userId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Failed to validate JWT token")]
    partial void LogTokenValidationFailed(Exception ex);
}
