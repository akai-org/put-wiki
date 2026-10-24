using System;

using Application.Abstractions;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure.Auth;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId =>
        Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                   throw new UnauthorizedAccessException());
}