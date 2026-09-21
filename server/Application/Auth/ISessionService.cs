using System;
using System.Threading;
using System.Threading.Tasks;

using FluentResults;

namespace Application.Auth;

public interface ISessionService
{
    Task<Result<TokenPair>> CreateAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<TokenPair>> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeAsync(string refreshToken, CancellationToken cancellationToken = default);
}