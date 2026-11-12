using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Infrastructure;
using Infrastructure.Auth;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public sealed class JwtUserSessionServiceTest : IDisposable
{
    private const string ValidSecretKey = "secret-key-that-is-32-bytes-long";
    private const string ValidIssuer = "valid-issuer";
    private const string ValidAudience = "valid-audience";

    private readonly JwtUserSessionServiceTestFixture _fixture = new();
    private readonly FakeTimeProvider _fakeTimeProvider = new();

    public void Dispose()
    {
        _fixture.Dispose();
    }

    private (JwtUserSessionService Sut, AppDbContext Context) CreateSut(
        string secretKey = ValidSecretKey,
        string issuer = ValidIssuer,
        string audience = ValidAudience,
        TimeSpan? accessTokenLifetime = null,
        TimeSpan? refreshTokenLifetime = null)
    {
        return _fixture.CreateSut(_fakeTimeProvider, secretKey, issuer, audience, accessTokenLifetime, refreshTokenLifetime);
    }

    [Fact]
    public async Task CreateSession_GivenValidUserId_ReturnsValidJwtAndRefreshTokenPair()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var (sut, context) = CreateSut();

        // Act
        var result = sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();

        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);
        jwtToken.Subject.Should().Be(user.Id.ToString());
    }

    [Fact]
    public async Task CreateSession_GivenValidUserId_PersistsRefreshToken()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var (sut, context) = CreateSut();

        // Act
        var result = sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        var refreshToken = await context.RefreshTokens.SingleAsync(rt =>
            rt.UserId == user.Id &&
            rt.Token == result.RefreshToken &&
            rt.Id != Guid.Empty);

        refreshToken.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateSession_GivenConfiguredLifetimes_SetsCorrectAccessTokenExpirationTime()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var accessTokenLifetime = TimeSpan.FromMinutes(45);
        var fixedTime = new DateTimeOffset(2026, 10, 22, 23, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut(accessTokenLifetime: accessTokenLifetime);

        // Act
        var result = sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);

        var expectedExpiration = fixedTime.Add(accessTokenLifetime).UtcDateTime;
        jwtToken.ValidTo.Should().BeCloseTo(expectedExpiration, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task CreateSession_GivenConfiguredLifetimes_SetsCorrectRefreshTokenExpirationAndCreationDates()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var refreshTokenLifetime = TimeSpan.FromDays(14);
        var fixedTime = new DateTimeOffset(2026, 10, 22, 23, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut(refreshTokenLifetime: refreshTokenLifetime);

        // Act
        sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        var savedRefreshToken = await context.RefreshTokens.SingleAsync(rt => rt.UserId == user.Id);
        savedRefreshToken.CreatedAt.Should().Be(fixedTime);
        savedRefreshToken.ExpiresAt.Should().Be(fixedTime.Add(refreshTokenLifetime));
    }

    [Fact]
    public async Task CreateSession_WhenCalledMultipleTimesForSameUser_GeneratesUniqueRefreshTokens()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var (sut, context) = CreateSut();

        // Act
        var session1 = sut.CreateSession(user.Id);
        var session2 = sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        session1.RefreshToken.Should().NotBe(session2.RefreshToken);
    }

    [Fact]
    public async Task CreateSession_WhenCalledMultipleTimesForSameUser_StoresAllRefreshTokensNotOverwritesThem()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var (sut, context) = CreateSut();

        // Act
        sut.CreateSession(user.Id);
        sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        var refreshTokens = await context.RefreshTokens.Where(rt => rt.UserId == user.Id).ToListAsync();
        refreshTokens.Count.Should().Be(2);
        refreshTokens[0].Id.Should().NotBe(refreshTokens[1].Id);
    }

    [Fact]
    public async Task CreateSession_WhenConfiguredWithIssuerAndAudience_EmbedsClaimsInJwt()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var expectedIssuer = "custom issuer";
        var expectedAudience = "custom audience";
        var (sut, context) = CreateSut(issuer: expectedIssuer, audience: expectedAudience);

        // Act
        var result = sut.CreateSession(user.Id);
        await context.SaveChangesAsync();

        // Assert
        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);

        jwtToken.Issuer.Should().Be(expectedIssuer);
        jwtToken.Audiences.Should().Contain(expectedAudience);
    }

    [Fact]
    public async Task RefreshSession_GivenValidRefreshToken_ShouldRevokeIt()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var refreshTokenString = "valid-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = false
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        // Act
        await sut.RefreshSession(refreshTokenString);
        await context.SaveChangesAsync();

        // Assert
        refreshToken.IsRevoked.Should().BeTrue();
    }

    [Fact]
    public async Task RefreshSession_GivenValidRefreshToken_ShouldReturnNewTokenPair()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var refreshTokenString = "valid-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = false
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.RefreshSession(refreshTokenString);
        await context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBe(refreshTokenString);
    }

    [Fact]
    public async Task RefreshSession_GivenInvalidRefreshTokenString_ShouldReturnNull()
    {
        // Arrange
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut();

        // Act
        var result = await sut.RefreshSession("non-existent-token");
        await context.SaveChangesAsync();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RefreshSession_GivenRevokedRefreshToken_ShouldRevokeAllUserTokensAndReturnNull()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var notRevokedRefreshTokenString = "refresh-token-1";
        var revokedRefreshTokenString = "refresh-token-2";

        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut();

        var notRevokedRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = notRevokedRefreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = false
        };
        var revokedRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = revokedRefreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = true
        };
        context.Add(notRevokedRefreshToken);
        context.Add(revokedRefreshToken);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.RefreshSession(revokedRefreshTokenString);
        await context.SaveChangesAsync();

        // Assert
        var refreshTokens = await context.RefreshTokens.Where(rt => rt.UserId == user.Id && rt.IsRevoked).ToListAsync();
        refreshTokens.Count.Should().Be(2);

        result.Should().BeNull();
    }

    [Fact]
    public async Task RefreshSession_GivenExpiredRefreshToken_ShouldReturnNull()
    {
        // Arrange
        var user = await _fixture.CreateUserAsync(_fakeTimeProvider);
        var refreshTokenString = "expired-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var (sut, context) = CreateSut();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-10),
            ExpiresAt = fixedTime.AddDays(-1),
            IsRevoked = false
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.RefreshSession(refreshTokenString);
        await context.SaveChangesAsync();

        // Assert
        result.Should().BeNull();
    }
}