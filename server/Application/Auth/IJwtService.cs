using System;
using System.Threading;
using System.Threading.Tasks;

using FluentResults;

namespace Application.Auth;

public interface IJwtService
{
    Task<Result<string>> GenerateTokenAsync(Guid userId, CancellationToken ct = default);
    Task<Result<Guid>> ValidateTokenAsync(string token, CancellationToken ct = default);
}
