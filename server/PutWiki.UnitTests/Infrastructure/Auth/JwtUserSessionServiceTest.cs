using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Infrastructure;
using Infrastructure.Auth;
using Infrastructure.Auth.Configuration;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public sealed class JwtUserSessionServiceTest : IDisposable
{
    private const string ValidSecretKey = "secret-key-that-is-32-bytes-long";
    private const string ValidIssuer = "putwiki-api";
    private const string ValidAudience = "putwiki-frontend";

    private readonly FakeTimeProvider _fakeTimeProvider;

    private readonly DbConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;

    public JwtUserSessionServiceTest()
    {
        _fakeTimeProvider = new FakeTimeProvider();

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    AppDbContext CreateContext() => new AppDbContext(_contextOptions);

    public void Dispose()
    {
        _connection.Dispose();
    }

    private static IOptions<JwtSettings> CreateOptions(
        string secretKey = ValidSecretKey,
        string issuer = ValidIssuer,
        string audience = ValidAudience,
        TimeSpan? accessTokenLifetime = null,
        TimeSpan? refreshTokenLifetime = null)
    {
        var settings = new JwtSettings
        {
            SecretKey = secretKey,
            Issuer = issuer,
            Audience = audience,
            AccessTokenLifetime = accessTokenLifetime ?? TimeSpan.FromMinutes(30),
            RefreshTokenLifetime = refreshTokenLifetime ?? TimeSpan.FromDays(7)
        };

        return Options.Create(settings);
    }

    [Fact]
    public async Task CreateSession_GivenValidUserId_ReturnsValidJwtAndRefreshTokenPair()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        using var context = CreateContext();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);
        await context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();

        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);
        jwtToken.Subject.Should().Be(userId.ToString());
    }

    [Fact]
    public async Task CreateSession_GivenValidUserId_PersistsRefreshToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        using var context = CreateContext();

        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);
        await context.SaveChangesAsync();

        // Assert
        var refreshToken = await context.RefreshTokens.SingleAsync(rt =>
            rt.UserId == userId &&
            rt.Token == result.RefreshToken &&
            rt.Id != Guid.Empty);

        refreshToken.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateSession_GivenConfiguredLifetimes_SetsCorrectAccessTokenExpirationTime()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accessTokenLifetime = TimeSpan.FromMinutes(45);
        var options = CreateOptions(accessTokenLifetime: accessTokenLifetime);
        using var context = CreateContext();

        var fixedTime = new DateTimeOffset(2026, 10, 22, 23, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);
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
        var userId = Guid.NewGuid();
        var refreshTokenLifetime = TimeSpan.FromDays(14);
        var options = CreateOptions(refreshTokenLifetime: refreshTokenLifetime);
        using var context = CreateContext();

        var fixedTime = new DateTimeOffset(2026, 10, 22, 23, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        sut.CreateSession(userId);
        await context.SaveChangesAsync();

        // Assert
        var savedRefreshToken = await context.RefreshTokens.SingleAsync(rt => rt.UserId == userId);
        savedRefreshToken.CreatedAt.Should().Be(fixedTime);
        savedRefreshToken.ExpiresAt.Should().Be(fixedTime.Add(refreshTokenLifetime));
    }

    [Fact]
    public async Task CreateSession_WhenCalledMultipleTimesForSameUser_GeneratesUniqueRefreshTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        using var context = CreateContext();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var session1 = sut.CreateSession(userId);
        var session2 = sut.CreateSession(userId);
        await context.SaveChangesAsync();

        // Assert
        session1.RefreshToken.Should().NotBe(session2.RefreshToken);
    }

    [Fact]
    public async Task CreateSession_WhenCalledMultipleTimesForSameUser_StoresAllRefreshTokensNotOverwritesThem()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        using var context = CreateContext();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        sut.CreateSession(userId);
        sut.CreateSession(userId);
        await context.SaveChangesAsync();

        // Assert
        var refreshTokens = await context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        refreshTokens.Count.Should().Be(2);
        refreshTokens[0].Id.Should().NotBe(refreshTokens[1].Id);
    }

    [Fact]
    public async Task CreateSession_WhenConfiguredWithIssuerAndAudience_EmbedsClaimsInJwt()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedIssuer = "custom issuer";
        var expectedAudience = "custom audience";
        var options = CreateOptions(issuer: expectedIssuer, audience: expectedAudience);
        using var context = CreateContext();

        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);
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
        var userId = Guid.NewGuid();
        var refreshTokenString = "valid-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);
        using var context = CreateContext();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = false
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        await sut.RefreshSession(refreshTokenString);

        // Assert
        refreshToken.IsRevoked.Should().BeTrue();
    }


    [Fact]
    public async Task RefreshSession_GivenValidRefreshToken_ShouldReturnNewTokenPair()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokenString = "valid-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);
        using var context = CreateContext();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = false
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = await sut.RefreshSession(refreshTokenString);

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
        using var context = CreateContext();

        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = await sut.RefreshSession("non-existent-token");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RefreshSession_GivenRevokedRefreshToken_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokenString = "revoked-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);
        using var context = CreateContext();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-1),
            ExpiresAt = fixedTime.AddDays(5),
            IsRevoked = true
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = await sut.RefreshSession(refreshTokenString);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RefreshSession_GivenExpiredRefreshToken_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokenString = "expired-refresh-token";
        var fixedTime = new DateTimeOffset(2026, 10, 24, 19, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);
        using var context = CreateContext();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenString,
            CreatedAt = fixedTime.AddDays(-10),
            ExpiresAt = fixedTime.AddDays(-1),
            IsRevoked = false
        };
        context.Add(refreshToken);
        await context.SaveChangesAsync();

        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, context, _fakeTimeProvider);

        // Act
        var result = await sut.RefreshSession(refreshTokenString);

        // Assert
        result.Should().BeNull();
    }
}