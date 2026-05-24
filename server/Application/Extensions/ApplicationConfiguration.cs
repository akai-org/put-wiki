using Application.Mappings;
using Application.Users;

using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingsProfile>();
        });

        services.AddScoped<ProvisionUserUseCase>();

        return services;
    }
}