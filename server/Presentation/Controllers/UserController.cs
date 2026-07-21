using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !System.Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var profile = new UserProfileDto
            {
                UserId = userId.ToString(),
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                AuthenticationType = User.Identity?.AuthenticationType ?? "Unknown"
            };

            return Ok(profile);
        }
    }

    public record UserProfileDto
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public string AuthenticationType { get; set; } = string.Empty;
    }
}