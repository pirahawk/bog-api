using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Ping()
        {
            return NoContent();
        }
    }
}