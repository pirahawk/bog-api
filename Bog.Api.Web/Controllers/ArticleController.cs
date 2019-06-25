using Bog.Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [Route("api/article")]
    public class ArticleController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody]NewEntryRequest newEntry)
        {
            return NoContent();
        }
    }
}