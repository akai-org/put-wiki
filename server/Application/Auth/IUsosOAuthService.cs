using System.Threading;
using System.Threading.Tasks;

using Application.Core;
using Application.DTOs;

namespace Application.Auth;

public interface IUsosOAuthService
{
    Task<Result<string>> GetLoginUrlAsync(CancellationToken cancellationToken = default);

    Task<Result<UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        CancellationToken cancellationToken = default
    );
}