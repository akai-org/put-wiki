using System.Threading;
using System.Threading.Tasks;

using Application.Features.Lecturers.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class LecturersController(
    GetLecturerQueryHandler getLecturerQueryHandler) : BaseApiController
{
    [HttpGet("~/lecturers/{identifier}")]
    [ProducesResponseType(typeof(LecturerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetLecturer(string identifier, CancellationToken ct)
    {
        var result = await getLecturerQueryHandler.ExecuteAsync(new GetLecturerQuery(identifier), ct);

        return HandleResult(result);
    }
}