using Application.Core;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (!result.IsSuccess && result.Code == 404)
                return NotFound();

            if (result.IsSuccess && result.Value is not null)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }
    }
}
