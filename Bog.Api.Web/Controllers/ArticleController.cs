using System;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [Route("api/article")]
    public class ArticleController : Controller
    {
        private readonly ICreateBlogEntryCoordinator _createBlogEntryCoordinator;

        public ArticleController(ICreateBlogEntryCoordinator createBlogEntryCoordinator)
        {
            _createBlogEntryCoordinator = createBlogEntryCoordinator;
        }

        [HttpPost]
        public IActionResult Create([FromBody]NewEntryRequest newEntry)
        {
            if (newEntry == null)
            {
                return BadRequest();
            }

            _createBlogEntryCoordinator.CreateNewEntry(newEntry);

            return NoContent();
        }
    }
}