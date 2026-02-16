using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("/")]
public class ExampleController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("Hello World!");
    }
}