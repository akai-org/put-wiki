using System;
using System.Data.Common;
using System.Threading.Tasks;

using Domain.Users;

using Infrastructure;
using Infrastructure.Auth;
using Infrastructure.Auth.Configuration;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public sealed class JwtUserSessionServiceTestFixture : IDisposable
{
    private const string DefaultSecretKey = "secret-key-that-is-32-bytes-long";
    private const string DefaultIssuer = "default-issuer";
    private const string DefaultAudience = "default-audience";

    private readonly DbConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;

    public JwtUserSessionServiceTestFixture()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public AppDbContext CreateContext() => new AppDbContext(_contextOptions);

    public (JwtUserSessionService Sut, AppDbContext Context) CreateSut(
        TimeProvider timeProvider,
        string secretKey = DefaultSecretKey,
        string issuer = DefaultIssuer,
        string audience = DefaultAudience,
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

        var context = CreateContext();
        var sut = new JwtUserSessionService(
            Options.Create(settings),
            context,
            timeProvider,
            NullLogger<JwtUserSessionService>.Instance);

        return (sut, context);
    }

    public async Task<User> CreateUserAsync(TimeProvider timeProvider, string usosId = "hashed-usos-id")
    {
        using var context = CreateContext();
        var user = User.Create(usosId, timeProvider.GetUtcNow());
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}