using Application.DTOs;

using AutoMapper;

using Domain.Users;

namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}