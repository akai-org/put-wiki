using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Errors;
using Application.Features.Users.Commands.ProvisionUser;
using Application.Features.Users.Commands.RefreshUserSession;

using FluentResults;

using Infrastructure.Auth.Configuration;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Presentation.Extensions;

using Scalar.AspNetCore;

namespace Presentation.Controllers;

public partial class AuthController(
    IUsosOAuthService usosOAuthService,
    ProvisionUserCommandHandler provisionUserCommandHandler,
    RefreshSessionCommandHandler refreshSessionCommandHandler,
    IOptions<JwtSettings> jwtSettings,
    ILogger<AuthController> logger
    ) : BaseApiController
{
    private const string OAuthStateCookieName = "oauth_state";

    [HttpGet("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Login(CancellationToken ct)
    {
        var result = await usosOAuthService.GetLoginUrlAsync(ct);
        if (result.IsFailed)
            return HandleResult(result);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(30)
        };
        Response.Cookies.Append(OAuthStateCookieName, result.Value.RequestToken, cookieOptions);

        return Redirect(result.Value.AuthorizeUrl);
    }

    [HttpGet("callback")]
    [ExcludeFromApiReference]
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
        if (!Request.Cookies.TryGetValue(OAuthStateCookieName, out var savedToken) || string.IsNullOrEmpty(savedToken))
        {
            LogPossibleCsrfAttack("Missing state cookie");
            return HandleResult(new UnauthorizedError("Missing OAuth state cookie."));
        }

        if (!string.Equals(savedToken, oauthToken, StringComparison.Ordinal))
        {
            LogPossibleCsrfAttack("State cookie mismatch");
            return HandleResult(new UnauthorizedError("OAuth state cookie mismatch."));
        }

        Response.Cookies.Delete(OAuthStateCookieName);

        var command = new ProvisionUserCommand(oauthToken, oauthVerifier);
        var result = await provisionUserCommandHandler.ExecuteAsync(command, ct);

        if (result.IsFailed)
        {
            return HandleResult(result);
        }

        AppendTokenCookies(result.Value);

        return HandleResult(Result.Ok());
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        var success = Request.Cookies.TryGetValue(AuthConstants.RefreshTokenCookieName, out string? refreshToken);
        if (!success || refreshToken is null)
        {
            return HandleResult(new UnauthorizedError("Refresh token not found"));
        }

        var command = new RefreshSessionCommand(refreshToken);
        var result = await refreshSessionCommandHandler.ExecuteAsync(command, ct);

        if (result.IsFailed)
        {
            return HandleResult(result);
        }

        AppendTokenCookies(result.Value);

        return HandleResult(Result.Ok());
    }

    private void AppendTokenCookies(JwtTokenPair tokenPair)
    {
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.Add(jwtSettings.Value.AccessTokenLifetime)
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.Add(jwtSettings.Value.RefreshTokenLifetime),
            Path = "/api/auth/refresh"
        };

        Response.Cookies.Append(AuthConstants.AccessTokenCookieName, tokenPair.AccessToken, accessTokenOptions);
        Response.Cookies.Append(AuthConstants.RefreshTokenCookieName, tokenPair.RefreshToken, refreshTokenOptions);
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Detected possible CSRF attack. Reason: {reason}")]
    private partial void LogPossibleCsrfAttack(string reason);
}