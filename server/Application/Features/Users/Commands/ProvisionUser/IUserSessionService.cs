using System;

namespace Application.Features.Users.Commands.ProvisionUser;

public interface IUserSessionService
{
    JwtTokenPair CreateSession(Guid userId);
}