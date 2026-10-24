using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Features.Users.Commands.ProvisionUser;

using FluentResults;

using Infrastructure.Auth.Configuration;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Presentation.Extensions;

namespace Presentation.Controllers;

public class AuthController(
    IUsosOAuthService usosOAuthService,
    ProvisionUserCommandHandler provisionUserCommandHandler,
    IOptions<JwtSettings> jwtSettings,
    IHostEnvironment environment) : BaseApiController
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
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
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
        var result = await provisionUserCommandHandler.ExecuteAsync(command, ct);

        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = environment.IsProduction(),
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.Add(jwtSettings.Value.AccessTokenLifetime)
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = environment.IsProduction(),
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.Add(jwtSettings.Value.RefreshTokenLifetime),
            Path = "/api/auth/refresh"
        };

        Response.Cookies.Append(AuthConstants.AccessTokenCookieName, result.Value.AccessToken, accessTokenOptions);
        Response.Cookies.Append(AuthConstants.RefreshTokenCookieName, result.Value.RefreshToken, refreshTokenOptions);

        return HandleResult(Result.Ok());
    }
}