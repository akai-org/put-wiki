using System.Linq;

using Application.Auth;

using Infrastructure.Auth;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

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
}