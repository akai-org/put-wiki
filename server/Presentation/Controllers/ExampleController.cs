using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("/")]
public class ExampleController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello World!";
    }
}