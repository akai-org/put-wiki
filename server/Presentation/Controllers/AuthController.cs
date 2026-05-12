using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers;

public class AuthController : BaseApiController
{
    private static readonly Action<ILogger, string?, Exception?> LogLoginUrlFailed = LoggerMessage.Define<string?>(
        LogLevel.Error,
        new EventId(1, nameof(LogLoginUrlFailed)),
        "Failed to get USOS login URL. Error: {Error}"
    );

    private static readonly Action<ILogger, string?, Exception?> LogCallbackFailed = LoggerMessage.Define<string?>(
        LogLevel.Warning,
        new EventId(2, nameof(LogCallbackFailed)),
        "USOS callback failed. Error: {Error}"
    );

    private readonly IUsosOAuthService _usosOAuthService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUsosOAuthService usosOAuthService, ILogger<AuthController> logger)
    {
        _usosOAuthService = usosOAuthService;
        _logger = logger;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(CancellationToken cancellationToken)
    {
        var result = await _usosOAuthService.GetLoginUrlAsync(cancellationToken);
        if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.Value))
        {
            LogLoginUrlFailed(_logger, result.Error, null);
            return StatusCode(result.Code == 0 ? 500 : result.Code, result.Error);
        }

        return Redirect(result.Value);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        [FromQuery(Name = "oauth_token")] string oauthToken,
        [FromQuery(Name = "oauth_verifier")] string oauthVerifier,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrEmpty(oauthToken) || string.IsNullOrEmpty(oauthVerifier))
        {
            return BadRequest("Missing OAuth parameters.");
        }

        var result = await _usosOAuthService.HandleCallbackAndGetUserAsync(oauthToken, oauthVerifier, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            LogCallbackFailed(_logger, result.Error, null);
            return StatusCode(result.Code == 0 ? 400 : result.Code, result.Error);
        }

        return Ok(result.Value);
    }
}