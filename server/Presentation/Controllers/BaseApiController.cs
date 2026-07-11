using System.Collections.Generic;
using System.Linq;

using Application.Errors;

using FluentResults;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value is null ? NoContent() : Ok(result.Value);
        }

        return HandleError(result.Errors);
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess) return Ok();
        return HandleError(result.Errors);
    }

    private ObjectResult HandleError(IReadOnlyList<IError> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError switch
        {
            NotFoundError => StatusCodes.Status404NotFound,
            ValidationError => StatusCodes.Status400BadRequest,
            UnauthorizedError => StatusCodes.Status401Unauthorized,
            ExternalServiceError => StatusCodes.Status502BadGateway,
            _ => StatusCodes.Status500InternalServerError
        };

        var extensions = new Dictionary<string, object?>
        {
            { "errors", errors.Select(e => e.Message).ToArray() }
        };

        return Problem(
            statusCode: statusCode,
            title: GetTitle(statusCode),
            detail: firstError.Message,
            extensions: extensions
        );
    }

    private static string GetTitle(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        404 => "Not Found",
        502 => "Bad Gateway",
        _ => "Server Error"
    };
}