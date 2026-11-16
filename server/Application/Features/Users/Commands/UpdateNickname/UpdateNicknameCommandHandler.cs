using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Abstractions;
using Application.Errors;

using Domain.Users;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Commands.UpdateNickname;

public partial class UpdateNicknameCommandHandler(
    IUserContext userContext,
    IUserRepository userRepository,
    ILogger<UpdateNicknameCommandHandler> logger,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> ExecuteAsync(UpdateNicknameCommand cmd,
    CancellationToken ct = default)
    {
        Guid userId = userContext.UserId;

        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return Result.Fail(new NotFoundError($"User with ID '{userId}' was not found."));
        }

        if (user.Nickname?.Value == cmd.Nickname)
        {
            return Result.Ok();
        }

        var updateResult = user.UpdateNickname(cmd.Nickname);
        if (updateResult.IsFailed)
        {
            return Result.Fail(new ValidationError("Failed to update nickname")
                .CausedBy(updateResult.Errors));
        }

        var nicknameTaken = await userRepository.ExistsWithNicknameAsync(cmd.Nickname, userId, ct);
        if (nicknameTaken)
        {
            return Result.Fail(new ConflictError("This nickname is already taken."));
        }

        await unitOfWork.SaveChangesAsync(ct);

        LogNicknameUpdated(userId);

        return Result.Ok();
    }

    [LoggerMessage(LogLevel.Information, "User {userId} updated their nickname")]
    partial void LogNicknameUpdated(Guid userId);
}