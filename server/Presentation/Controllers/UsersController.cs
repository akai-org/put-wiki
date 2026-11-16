using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.Users.Commands.UpdateNickname;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UsersController(
    UpdateNicknameCommandHandler updateNicknameCommandHandler) : BaseApiController
{
    [Authorize]
    [HttpPatch("me/nickname")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateNickname(
        [FromBody] UpdateNicknameRequest request,
        CancellationToken ct)
    {
        var command = new UpdateNicknameCommand(request.Nickname);
        var result = await updateNicknameCommandHandler.ExecuteAsync(command, ct);

        return HandleResult(result);
    }
}

public sealed record UpdateNicknameRequest(string Nickname);