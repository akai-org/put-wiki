using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.Users.Commands.UpdateNickname;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    UpdateNicknameCommandHandler updateNicknameCommandHandler) : BaseApiController
{
    [HttpPatch("{userId:guid}")]
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
        var result = await updateNicknameCommandHandler.ExecuteAsync(command, ct);

        return HandleResult(result);
    }
}

public sealed record UpdateNicknameRequest(string Nickname);