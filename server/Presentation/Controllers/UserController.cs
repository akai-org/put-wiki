using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.Users.Commands.UpdateNickname;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class UserController(
    UpdateNicknameUseCase updateNicknameUseCase) : BaseApiController
{
    [HttpPatch("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateNickname(
        Guid userId,
        [FromBody] JsonElement body,
        CancellationToken ct)
    {
        var request = body.Deserialize<UpdateNicknameRequest>(JsonSerializerOptions.Web);
        if (request is null)
            return BadRequest();

        var command = new UpdateNicknameCommand(userId, request.Nickname);
        var result = await updateNicknameUseCase.ExecuteAsync(command, ct);

        return HandleResult(result);
    }

    private sealed record UpdateNicknameRequest(string Nickname);
}