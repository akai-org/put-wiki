using System;

namespace Application.Features.Users.Commands.ProvisionUser;

public interface IUserSessionService
{
    (string AccessToken, string RefreshToken) CreateSession(Guid userId);
}