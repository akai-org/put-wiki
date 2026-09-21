using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Errors;

using Domain.Users;

using FluentResults;

using Infrastructure.Auth.Configuration;

using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

public class SessionService(
    IJwtService jwtService,
    IRefreshSessionRepository refreshSessionRepository,
    IOptions<JwtSettings> jwtSettings,
    TimeProvider timeProvider) : ISessionService
{
    private readonly JwtSettings _settings = jwtSettings.Value;

    public async Task<Result<TokenPair>> CreateAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var accessTokenResult = await jwtService.GenerateAccessTokenAsync(userId, cancellationToken);
        if (accessTokenResult.IsFailed)
            return Result.Fail(accessTokenResult.Errors);

        var refreshToken = GenerateRefreshToken();
        var now = timeProvider.GetUtcNow();
        refreshSessionRepository.Add(new RefreshSession(
            userId,
            HashRefreshToken(refreshToken),
            now,
            now.AddDays(_settings.RefreshTokenExpirationDays)));
        await refreshSessionRepository.SaveChangesAsync(cancellationToken);

        return Result.Ok(new TokenPair(accessTokenResult.Value, refreshToken));
    }

    public async Task<Result<TokenPair>> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result.Fail(new UnauthorizedError("Refresh token is missing."));

        var session = await refreshSessionRepository.GetByTokenHashAsync(
            HashRefreshToken(refreshToken), cancellationToken);
        var now = timeProvider.GetUtcNow();
        if (session is null || !session.IsActive(now))
            return Result.Fail(new UnauthorizedError("Refresh token is invalid or expired."));

        var accessTokenResult = await jwtService.GenerateAccessTokenAsync(session.UserId, cancellationToken);
        if (accessTokenResult.IsFailed)
            return Result.Fail(accessTokenResult.Errors);

        var replacementToken = GenerateRefreshToken();
        var replacement = new RefreshSession(
            session.UserId,
            HashRefreshToken(replacementToken),
            now,
            now.AddDays(_settings.RefreshTokenExpirationDays));
        var revoked = await refreshSessionRepository.TryRevokeAsync(
            session.Id,
            now,
            replacement.Id,
            cancellationToken);
        if (!revoked)
            return Result.Fail(new UnauthorizedError("Refresh token has already been used."));

        refreshSessionRepository.Add(replacement);
        await refreshSessionRepository.SaveChangesAsync(cancellationToken);

        return Result.Ok(new TokenPair(accessTokenResult.Value, replacementToken));
    }

    public async Task<Result> RevokeAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result.Ok();

        var session = await refreshSessionRepository.GetByTokenHashAsync(
            HashRefreshToken(refreshToken), cancellationToken);
        if (session is not null && session.RevokedAt is null)
        {
            await refreshSessionRepository.TryRevokeAsync(
                session.Id,
                timeProvider.GetUtcNow(),
                null,
                cancellationToken);
        }

        return Result.Ok();
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static string HashRefreshToken(string refreshToken)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
    }
}