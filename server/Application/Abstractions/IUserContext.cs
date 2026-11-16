using System;

namespace Application.Abstractions;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
}