using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Users;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers;

public partial class AuthController : BaseApiController
{
    private readonly IUsosOAuthService _usosOAuthService;
    private readonly ProvisionUserUseCase _provisionUserUseCase;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUsosOAuthService usosOAuthService,
        ProvisionUserUseCase provisionUserUseCase,
        ILogger<AuthController> logger)
    {
        _usosOAuthService = usosOAuthService;
        _provisionUserUseCase = provisionUserUseCase;
        _logger = logger;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(CancellationToken cancellationToken)
    {
        var result = await _usosOAuthService.GetLoginUrlAsync(cancellationToken);
        if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.Value))
        {
            var errorMsg = result.Error ?? "Unknown USOS authentication error.";
            LogLoginUrlFailed(errorMsg);
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

        var result = await _provisionUserUseCase.ExecuteAsync(oauthToken, oauthVerifier, cancellationToken);

        if (!result.IsSuccess || result.Value is null)
        {
            var errorMsg = result.Error ?? "Unknown internal authentication error.";
            LogCallbackFailed(errorMsg);
            return StatusCode(result.Code == 0 ? 500 : result.Code, result.Error);
        }

        return Ok(result.Value);
    }

    [LoggerMessage(LogLevel.Error, "Failed to get USOS login URL. Error: {error}")]
    partial void LogLoginUrlFailed(string error);

    [LoggerMessage(LogLevel.Warning, "USOS callback failed. Error: {error}")]
    partial void LogCallbackFailed(string error);
}