using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Errors;

using Domain.Users;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Commands.UpdateNickname;

public partial class UpdateNicknameUseCase(
    IUserRepository userRepository,
    ILogger<UpdateNicknameUseCase> logger)
{

    public async Task<Result> ExecuteAsync(UpdateNicknameCommand cmd,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(cmd.UserId, ct);
        if (user is null)
        {
            return Result.Fail(new NotFoundError($"User with ID '{cmd.UserId}' was not found."));
        }

        var updateResult = user.UpdateNickname(cmd.Nickname);
        if (updateResult.IsFailed)
        {
            return Result.Fail(new ValidationError(updateResult.Errors[0].Message));
        }

        var nicknameTaken = await userRepository.ExistsWithNicknameAsync(user.Nickname!, cmd.UserId, ct);
        if (nicknameTaken)
        {
            return Result.Fail(new ConflictError("This nickname is already taken."));
        }

        await userRepository.SaveChangesAsync(ct);

        LogNicknameUpdated(cmd.UserId, user.Nickname!);

        return Result.Ok();
    }

    [LoggerMessage(LogLevel.Information, "User {userId} updated their nickname to '{nickname}'")]
    partial void LogNicknameUpdated(Guid userId, string nickname);
}
