using System;
using System.Linq;
using System.Threading.Tasks;

using Application.Auth;

using Domain.Users;

using Infrastructure.Auth;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public static partial class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IUserRepository, UserRepository>();
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

        services.AddHttpClient<IUsosOAuthService, UsosOAuthService>();

        return services;
    }

    // NOTE: don't use this method in PRODUCTION enviroment to apply migrations during app startup.
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