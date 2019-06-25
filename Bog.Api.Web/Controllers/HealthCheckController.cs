using System.Linq;
using Bog.Api.Db.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [Route("api/ping")]
    public class HealthCheckController : Controller
    {
        [HttpGet()]
        public IActionResult Ping()
        {
            return NoContent();
        }
    }
}