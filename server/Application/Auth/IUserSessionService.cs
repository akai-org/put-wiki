using System;
using System.Threading.Tasks;

namespace Application.Auth;

public interface IUserSessionService
{
    JwtTokenPair CreateSession(Guid userId);
    Task<JwtTokenPair?> RefreshSession(string refreshToken);
}