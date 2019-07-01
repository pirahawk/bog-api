using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
        public async Task<IActionResult> CreateArticle([FromBody]ArticleRequest article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            var result = await _createBlogEntryCoordinator.CreateNewEntryAsync(article);

            if (result == null)
            {
                return BadRequest();
            }

            var response = MapNewArticleResponse(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetArticle(Guid id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}/entry")]
        public async Task<IActionResult> GetEntryCollection()
        {
            return Ok();
        }

        private ArticleResponse MapNewArticleResponse(Article result)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.SELF, Hred = Url.Action(nameof(GetArticle), new { id = result.Id})}, 
                new Link {Relation = LinkRelValueObject.GET_ENTRY_COLLECTION, Hred = Url.Action(nameof(GetEntryCollection), new { id = result.Id})}, 
            };

            return new ArticleResponse
            {
                Id = result.Id,
                BlogId = result.BlogId,
                Author = result.Author,
                Links = links
            };
        }
    }
}