using System.Threading;
using System.Threading.Tasks;

using Application.DTOs;

using FluentResults;

namespace Application.Auth;

public interface IUsosOAuthService
{
    Task<Result<string>> GetLoginUrlAsync(CancellationToken ct = default);

    Task<Result<UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        CancellationToken ct = default
    );
}