using System.Linq;
using Bog.Api.Db.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [Route("api/ping")]
    public class PingController : Controller
    {
        private readonly BlogApiDbContext _context;

        public PingController(BlogApiDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public IActionResult Ping()
        {
            var articles = _context.Articles.ToArray();
            return NoContent();
        }
    }
}