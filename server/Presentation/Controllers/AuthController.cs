using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Features.Users.Commands.ProvisionUser;

using Infrastructure.Auth.Configuration;
using Infrastructure.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Presentation.Controllers;

public class AuthController(
    IUsosOAuthService usosOAuthService,
    ProvisionUserUseCase provisionUserUseCase,
    IOptions<JwtSettings> jwtSettings,
    TimeProvider timeProvider) : BaseApiController
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Callback(
        [FromQuery(Name = "oauth_token")] string oauthToken,
        [FromQuery(Name = "oauth_verifier")] string oauthVerifier,
        CancellationToken ct
    )
    {
        var command = new ProvisionUserCommand(oauthToken, oauthVerifier);
        var result = await provisionUserUseCase.ExecuteAsync(command, ct);

        if (result.IsFailed)
            return HandleResult(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = timeProvider.GetUtcNow().AddMinutes(_jwtSettings.ExpirationMinutes),
            Path = "/",
            IsEssential = true,
        };

        Response.Cookies.Append(InfrastructureConfiguration.AuthCookieName, result.Value, cookieOptions);

        return Ok();
    }
}