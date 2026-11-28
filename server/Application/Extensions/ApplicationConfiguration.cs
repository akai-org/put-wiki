using Application.Features.AcademicTeachers.Queries;
using Application.Features.Users.Commands.ProvisionUser;
using Application.Features.Users.Commands.RefreshUserSession;
using Application.Features.Users.Commands.UpdateNickname;

using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ProvisionUserCommandHandler>();
        services.AddScoped<RefreshSessionCommandHandler>();

        services.AddScoped<GetAcademicTeacherQueryHandler>();
        services.AddScoped<UpdateNicknameCommandHandler>();

        return services;
    }
}