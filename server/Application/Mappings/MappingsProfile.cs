using Application.DTOs;

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