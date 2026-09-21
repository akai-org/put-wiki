using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;

using Domain.Users;

using FluentAssertions;

using FluentResults;

using Infrastructure.Auth;
using Infrastructure.Auth.Configuration;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;

using Moq;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public class SessionServiceTests
{
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IRefreshSessionRepository> _repositoryMock = new();
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly SessionService _sut;

    public SessionServiceTests()
    {
        _sut = new SessionService(
            _jwtServiceMock.Object,
            _repositoryMock.Object,
            Options.Create(new JwtSettings
            {
                Secret = "development-only-secret-with-at-least-32-characters",
                Issuer = "PutWiki",
                Audience = "PutWikiClient",
                AccessTokenExpirationMinutes = 15,
                RefreshTokenExpirationDays = 7,
            }),
            _timeProvider);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAccessTokenAndPersistRefreshSession()
    {
        var userId = Guid.NewGuid();
        var now = new DateTimeOffset(2026, 9, 21, 12, 0, 0, TimeSpan.Zero);
        _timeProvider.SetUtcNow(now);
        _jwtServiceMock
            .Setup(x => x.GenerateAccessTokenAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok("access-token"));

        var result = await _sut.CreateAsync(userId);

        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().NotBeNullOrWhiteSpace();
        _repositoryMock.Verify(x => x.Add(It.Is<RefreshSession>(session =>
            session.UserId == userId
            && session.CreatedAt == now
            && session.ExpiresAt == now.AddDays(7)
            && session.TokenHash != result.Value.RefreshToken)), Times.Once);
        _repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_ShouldRevokeCurrentSessionAndCreateReplacement()
    {
        var userId = Guid.NewGuid();
        var now = new DateTimeOffset(2026, 9, 21, 12, 0, 0, TimeSpan.Zero);
        var session = new RefreshSession(userId, "hashed-refresh-token", now, now.AddDays(7));
        _timeProvider.SetUtcNow(now.AddHours(1));
        _repositoryMock
            .Setup(x => x.GetByTokenHashAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);
        _repositoryMock
            .Setup(x => x.TryRevokeAsync(
                session.Id,
                It.IsAny<DateTimeOffset>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
            .Returns((Guid _, DateTimeOffset revokedAt, Guid? replacementId, CancellationToken _) =>
            {
                session.Revoke(revokedAt, replacementId);
                return Task.FromResult(true);
            });
        _jwtServiceMock
            .Setup(x => x.GenerateAccessTokenAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok("new-access-token"));

        var result = await _sut.RefreshAsync("refresh-token");

        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("new-access-token");
        session.IsActive(_timeProvider.GetUtcNow()).Should().BeFalse();
        _repositoryMock.Verify(x => x.Add(It.Is<RefreshSession>(replacement =>
            replacement.UserId == userId
            && replacement.Id != session.Id)), Times.Once);
        _repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_WhenRotationLosesRace_ShouldRejectReuse()
    {
        var now = _timeProvider.GetUtcNow();
        var session = new RefreshSession(
            Guid.NewGuid(),
            "hashed-refresh-token",
            now,
            now.AddDays(7));
        _repositoryMock
            .Setup(x => x.GetByTokenHashAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);
        _repositoryMock
            .Setup(x => x.TryRevokeAsync(
                session.Id,
                It.IsAny<DateTimeOffset>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _jwtServiceMock
            .Setup(x => x.GenerateAccessTokenAsync(session.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok("unused-access-token"));

        var result = await _sut.RefreshAsync("refresh-token");

        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be("Refresh token has already been used.");
        _repositoryMock.Verify(x => x.Add(It.IsAny<RefreshSession>()), Times.Never);
    }
}