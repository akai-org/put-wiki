using System;
using System.Threading.Tasks;

using Infrastructure.Auth;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class AuthController : BaseApiController
{
    private readonly UsosOAuthService _usosOAuthService;

    public AuthController(UsosOAuthService usosOAuthService)
    {
        _usosOAuthService = usosOAuthService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var url = await _usosOAuthService.GetLoginUrlAsync();
        return Redirect(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery(Name = "oauth_token")] string oauthToken, [FromQuery(Name = "oauth_verifier")] string oauthVerifier)
    {
        if (string.IsNullOrEmpty(oauthToken) || string.IsNullOrEmpty(oauthVerifier))
        {
            return BadRequest("Missing OAuth parameters.");
        }

        try
        {
            var user = await _usosOAuthService.HandleCallbackAndGetUserAsync(oauthToken, oauthVerifier);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}