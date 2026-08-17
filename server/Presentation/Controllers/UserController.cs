using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.Users.Commands.UpdateNickname;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UserController(
    UpdateNicknameUseCase updateNicknameUseCase) : BaseApiController
{
    // TODO: Replace userId route parameter with authenticated user ID from JWT claims
    // when the authentication middleware is implemented.
    [HttpPatch("{userId:guid}/nickname")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateNickname(
        Guid userId,
        [FromBody] UpdateNicknameRequest request,
        CancellationToken ct)
    {
        var command = new UpdateNicknameCommand(userId, request.Nickname);
        var result = await updateNicknameUseCase.ExecuteAsync(command, ct);

        return HandleResult(result);
    }
}

public record UpdateNicknameRequest(string Nickname);