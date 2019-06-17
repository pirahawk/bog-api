using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [Route("api/ping")]
    public class PingController : Controller
    {
        [HttpGet()]
        public IActionResult Ping()
        {
            return NoContent();
        }
    }
}