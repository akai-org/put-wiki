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