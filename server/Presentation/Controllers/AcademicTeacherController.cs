using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Errors;
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
    [ProducesResponseType(typeof(AcademicTeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAcademicTeacher(string id, CancellationToken ct)
    {
        bool res = Guid.TryParse(id, out Guid guid);
        if (!res)
            return HandleResult(new ValidationError("Provided academic teacher ID is invalid."));

        var result = await handler.ExecuteAsync(new GetAcademicTeacherQuery(guid), ct);

        return HandleResult(result);
    }
}