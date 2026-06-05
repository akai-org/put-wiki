using Application.Features.Users.Commands.ProvisionUser;

using AutoMapper;

using Domain.Users;

namespace Application.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<User, UserDto>();
    }
}