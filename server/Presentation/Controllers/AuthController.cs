using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.DTOs;
using Application.Users;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class AuthController(
    IUsosOAuthService usosOAuthService,
    ProvisionUserUseCase provisionUserUseCase) : BaseApiController
{

    [HttpGet("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Login(CancellationToken ct)
    {
        var result = await usosOAuthService.GetLoginUrlAsync(ct);
        if (result.IsSuccess)
            return Redirect(result.Value);

        return HandleResult(result);
    }

    [HttpGet("callback")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Callback(
        [FromQuery(Name = "oauth_token")] string oauthToken,
        [FromQuery(Name = "oauth_verifier")] string oauthVerifier,
        CancellationToken ct
    )
    {
        var result = await provisionUserUseCase.ExecuteAsync(oauthToken, oauthVerifier, ct);
        return HandleResult(result);
    }
}