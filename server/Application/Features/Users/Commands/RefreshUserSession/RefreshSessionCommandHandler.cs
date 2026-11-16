using System.Threading;
using System.Threading.Tasks;

using Application.Abstractions;
using Application.Auth;
using Application.Errors;

using FluentResults;

namespace Application.Features.Users.Commands.RefreshUserSession;

public class RefreshSessionCommandHandler(
    IUserSessionService userSessionService,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<JwtTokenPair>> ExecuteAsync(RefreshSessionCommand cmd, CancellationToken ct)
    {
        var newPair = await userSessionService.RefreshSession(cmd.RefreshToken);
        if (newPair is null)
        {
            return Result.Fail(new UnauthorizedError("Cannot refresh user's session"));
        }

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(newPair);
    }
}