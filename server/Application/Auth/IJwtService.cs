using System;
using System.Threading;
using System.Threading.Tasks;

using FluentResults;

namespace Application.Auth;

public interface IJwtService
{
    Task<Result<string>> GenerateAccessTokenAsync(Guid userId, CancellationToken ct = default);
}