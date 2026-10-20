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
        if (result.IsSuccess)
        {
            return Ok();
        }

        var errors = new List<string>();

        for (var i = 0; i < result.Errors.Count; i++)
        {
            var error = result.Errors[i];
            errors.Add(error.Message);

            for (var j = 0; j < error.Reasons.Count; j++)
            {
                if (error.Reasons[j] is IError reason)
                {
                    errors.Add(reason.Message);
                }
            }
        }

        var firstDetail = result.Errors.Count > 0 ? result.Errors[0].Message : null;

        var problemDetails = new ProblemDetails
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = firstDetail,
            Extensions =
        {
            ["errors"] = errors
        }
        };

        return BadRequest(problemDetails);
    }

    private ObjectResult HandleError(IReadOnlyList<IError> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError switch
        {
            NotFoundError => StatusCodes.Status404NotFound,
            ValidationError => StatusCodes.Status400BadRequest,
            UnauthorizedError => StatusCodes.Status401Unauthorized,
            ConflictError => StatusCodes.Status409Conflict,
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
        409 => "Conflict",
        502 => "Bad Gateway",
        _ => "Server Error"
    };
}