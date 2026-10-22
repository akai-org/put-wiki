using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Infrastructure;
using Infrastructure.Auth;
using Infrastructure.Auth.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.JsonWebTokens;

using Moq;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public class JwtUserSessionServiceTest
{
    private const string ValidSecretKey = "secret-key-that-is-32-bytes-long";
    private const string ValidIssuer = "putwiki-api";
    private const string ValidAudience = "putwiki-frontend";

    private readonly Mock<DbSet<RefreshToken>> _dbSetMock;
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly List<RefreshToken> _refreshTokenStore;

    public JwtUserSessionServiceTest()
    {
        _dbSetMock = new Mock<DbSet<RefreshToken>>();
        _fakeTimeProvider = new FakeTimeProvider();
        _refreshTokenStore = new List<RefreshToken>();

        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().Options;
        _dbContextMock = new Mock<AppDbContext>(dbContextOptions);

        _dbSetMock
            .Setup(x => x.Add(It.IsAny<RefreshToken>()))
            .Callback<RefreshToken>(x => _refreshTokenStore.Add(x));

        _dbContextMock
            .Setup(x => x.RefreshTokens)
            .Returns(_dbSetMock.Object);
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
    public void CreateSession_GivenValidUserId_ReturnsValidJwtAndRefreshTokenPair()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrWhiteSpace();
        result.RefreshToken.Should().NotBeNullOrWhiteSpace();

        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);
        jwtToken.Subject.Should().Be(userId.ToString());
    }

    [Fact]
    public void CreateSession_GivenValidUserId_PersistsRefreshToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);

        // Assert
        _dbSetMock.Verify(x => x.Add(It.Is<RefreshToken>(rt =>
            rt.UserId == userId &&
            rt.Token == result.RefreshToken &&
            rt.Id != Guid.Empty
        )), Times.Once);
    }

    [Fact]
    public void CreateSession_GivenConfiguredLifetimes_SetsCorrectAccessTokenExpirationTime()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accessTokenLifetime = TimeSpan.FromMinutes(45);
        var options = CreateOptions(accessTokenLifetime: accessTokenLifetime);

        var fixedTime = new DateTimeOffset(2026, 10, 22, 23, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);

        // Assert
        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);

        var expectedExpiration = fixedTime.Add(accessTokenLifetime).UtcDateTime;
        jwtToken.ValidTo.Should().BeCloseTo(expectedExpiration, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CreateSession_GivenConfiguredLifetimes_SetsCorrectRefreshTokenExpirationAndCreationDates()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokenLifetime = TimeSpan.FromDays(14);
        var options = CreateOptions(refreshTokenLifetime: refreshTokenLifetime);

        var fixedTime = new DateTimeOffset(2026, 10, 22, 23, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(fixedTime);

        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        sut.CreateSession(userId);

        // Assert
        var savedRefreshToken = _refreshTokenStore.Single(rt => rt.UserId == userId);
        savedRefreshToken.CreatedAt.Should().Be(fixedTime);
        savedRefreshToken.ExpiresAt.Should().Be(fixedTime.Add(refreshTokenLifetime));
    }

    [Fact]
    public void CreateSession_WhenCalledMultipleTimesForSameUser_GeneratesUniqueRefreshTokens()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        var session1 = sut.CreateSession(userId);
        var session2 = sut.CreateSession(userId);

        // Assert
        session1.RefreshToken.Should().NotBe(session2.RefreshToken);
    }

    [Fact]
    public void CreateSession_WhenCalledMultipleTimesForSameUser_StoresAllRefreshTokensNotOverwritesThem()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var options = CreateOptions();
        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        sut.CreateSession(userId);
        sut.CreateSession(userId);

        // Assert
        _dbSetMock.Verify(x => x.Add(It.Is<RefreshToken>(rt => rt.UserId == userId)), Times.Exactly(2));
        _refreshTokenStore.Where(rt => rt.UserId == userId).Should().HaveCount(2);
        _refreshTokenStore[0].Id.Should().NotBe(_refreshTokenStore[1].Id);
    }

    [Fact]
    public void CreateSession_WhenConfiguredWithIssuerAndAudience_EmbedsClaimsInJwt()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedIssuer = "custom issuer";
        var expectedAudience = "custom audience";
        var options = CreateOptions(issuer: expectedIssuer, audience: expectedAudience);

        var sut = new JwtUserSessionService(options, _dbContextMock.Object, _fakeTimeProvider);

        // Act
        var result = sut.CreateSession(userId);

        // Assert
        var handler = new JsonWebTokenHandler();
        var jwtToken = handler.ReadJsonWebToken(result.AccessToken);

        jwtToken.Issuer.Should().Be(expectedIssuer);
        jwtToken.Audiences.Should().Contain(expectedAudience);
    }

}