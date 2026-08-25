using System;
using System.Linq;
using System.Threading.Tasks;

using Application.Auth;
using Application.Features.Users.Commands.ProvisionUser;

using Domain.Users;

using Infrastructure.Auth;
using Infrastructure.Auth.Configuration;
using Infrastructure.Clients;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Extensions;

public static partial class InfrastructureConfiguration
{
    public const string AuthCookieName = "auth_token";
    private static readonly string[] MissingJwtSectionErrors = ["Jwt section is missing from configuration."];

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddHttpClient<IUsosHttpClient, UsosHttpClient>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddSingleton<IUsosIdHasher, HmacUsosIdHasher>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }

    public static IServiceCollection AddUsosOAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<UsosOAuthSettings>()
            .Bind(configuration.GetSection("UsosOAuth"))
            .Validate(settings =>
            {
                var validator = new UsosOAuthSettingsValidator();
                var validationResult = validator.Validate(settings);
                if (!validationResult.IsValid)
                {
                    throw new Microsoft.Extensions.Options.OptionsValidationException(
                        "UsosOAuthSettings",
                        typeof(UsosOAuthSettings),
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                }
                return true;
            })
            .ValidateOnStart();

        services.AddScoped<IUsosOAuthService, UsosOAuthService>();

        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("Jwt"))
            .Validate(settings =>
            {
                var validator = new JwtSettingsValidator();
                var validationResult = validator.Validate(settings);
                if (!validationResult.IsValid)
                {
                    throw new OptionsValidationException(
                        "JwtSettings",
                        typeof(JwtSettings),
                        validationResult.Errors.Select(e => e.ErrorMessage)
                    );
                }
                return true;
            })
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new OptionsValidationException(
                "JwtSettings",
                typeof(JwtSettings),
                MissingJwtSectionErrors);

        var signingKey = CreateSigningKey(jwtSettings.Secret);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = !string.IsNullOrWhiteSpace(jwtSettings.Issuer),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = !string.IsNullOrWhiteSpace(jwtSettings.Audience),
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (string.IsNullOrWhiteSpace(context.Token)
                            && context.Request.Cookies.TryGetValue(AuthCookieName, out var token)
                            && !string.IsNullOrWhiteSpace(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    private static SymmetricSecurityKey CreateSigningKey(string secret)
    {
        var key = Encoding.UTF8.GetBytes(secret);
        if (key.Length < 32)
            key = SHA256.HashData(key);

        return new SymmetricSecurityKey(key);
    }

    public static async Task<IApplicationBuilder> ApplyDatabaseMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
            LogAnErrorOccurredWhileMigratingTheDatabase(logger, ex);
            throw;
        }

        return app;
    }

    [LoggerMessage(Level = LogLevel.Critical, Message = "An error occurred while migrating the database.")]
    static partial void LogAnErrorOccurredWhileMigratingTheDatabase(ILogger logger, Exception ex);
}