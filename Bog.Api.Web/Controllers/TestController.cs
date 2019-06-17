using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpGet()]
        public IActionResult Test()
        {
            return Ok("This Works");
        }
    }
}