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

        return HandleError(result.Errors);
    }

    private ObjectResult HandleError(IReadOnlyList<IError> errors)
    {
        if (errors is null || errors.Count == 0)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: GetTitle(StatusCodes.Status500InternalServerError),
                detail: "An unknown error occurred."
            );
        }

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

        var errorMessages = new HashSet<string>();

        foreach (var error in errors)
        {
            if (!string.IsNullOrWhiteSpace(error.Message))
            {
                errorMessages.Add(error.Message);
            }

            if (error.Reasons is null) continue;

            foreach (var reason in error.Reasons)
            {
                if (!string.IsNullOrWhiteSpace(reason.Message))
                {
                    errorMessages.Add(reason.Message);
                }
            }
        }

        var extensions = new Dictionary<string, object?>
        {
            ["errors"] = errorMessages.ToArray()
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
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status404NotFound => "Not Found",
        StatusCodes.Status409Conflict => "Conflict",
        StatusCodes.Status502BadGateway => "Bad Gateway",
        _ => "Server Error"
    };
}