using System.Threading;
using System.Threading.Tasks;

using Application.Core;

namespace Application.Auth;

public interface IUsosOAuthService
{
    Task<Result<string>> GetLoginUrlAsync(CancellationToken cancellationToken = default);

    Task<Result<UsosUserDto>> HandleCallbackAndGetUserAsync(
        string oauthToken,
        string oauthVerifier,
        CancellationToken cancellationToken = default
    );

    public sealed record UsosUserDto(string Id, string FirstName, string LastName, string StudentNumber, string Email);
}
