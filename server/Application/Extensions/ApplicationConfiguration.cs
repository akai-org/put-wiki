using Application.Features.AcademicTeachers.Queries;
using Application.Features.Users.Commands.ProvisionUser;
using Application.Features.Users.Commands.UpdateNickname;
using Application.Mappings;

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

        services.AddScoped<ProvisionUserCommandHandler>();
        services.AddScoped<GetAcademicTeacherQueryHandler>();
        services.AddScoped<UpdateNicknameUseCase>();

        return services;
    }
}
