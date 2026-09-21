using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Features.Users.Commands.ProvisionUser;

using Infrastructure.Auth.Configuration;
using Infrastructure.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Presentation.Controllers;

[AllowAnonymous]
public class AuthController(
    IUsosOAuthService usosOAuthService,
    ProvisionUserUseCase provisionUserUseCase,
    ISessionService sessionService,
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

        var sessionResult = await sessionService.CreateAsync(result.Value, ct);
        if (sessionResult.IsFailed)
            return HandleResult(sessionResult);

        WriteSessionCookies(sessionResult.Value);

        return Ok();
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        if (!Request.Cookies.TryGetValue(InfrastructureConfiguration.RefreshCookieName, out var refreshToken))
            return Unauthorized();

        var result = await sessionService.RefreshAsync(refreshToken, ct);
        if (result.IsFailed)
            return HandleResult(result);

        WriteSessionCookies(result.Value);
        return Ok();
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        Request.Cookies.TryGetValue(InfrastructureConfiguration.RefreshCookieName, out var refreshToken);
        await sessionService.RevokeAsync(refreshToken ?? string.Empty, ct);
        Response.Cookies.Delete(InfrastructureConfiguration.AuthCookieName);
        Response.Cookies.Delete(InfrastructureConfiguration.RefreshCookieName);
        return NoContent();
    }

    private void WriteSessionCookies(TokenPair tokenPair)
    {
        var now = timeProvider.GetUtcNow();
        Response.Cookies.Append(
            InfrastructureConfiguration.AuthCookieName,
            tokenPair.AccessToken,
            CreateCookieOptions(now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)));
        Response.Cookies.Append(
            InfrastructureConfiguration.RefreshCookieName,
            tokenPair.RefreshToken,
            CreateCookieOptions(now.AddDays(_jwtSettings.RefreshTokenExpirationDays)));
    }

    private CookieOptions CreateCookieOptions(DateTimeOffset expiresAt) => new()
    {
        HttpOnly = true,
        Secure = Request.IsHttps,
        SameSite = SameSiteMode.Lax,
        Expires = expiresAt,
        Path = "/",
        IsEssential = true,
    };
}