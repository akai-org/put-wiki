using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.AcademicTeachers.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AcademicTeacherController(
    GetAcademicTeacherHandler handler) : BaseApiController
{
    [HttpGet("teachers/{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAcademicTeacher(string id, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(new GetAcademicTeacherQuery(new Guid(id)), ct);

        return HandleResult(result);
    }
}