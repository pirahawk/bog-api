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
        private readonly IFindBlogArticleCoordinator _findBlogArticleCoordinator;

        public ArticleController(ICreateBlogEntryCoordinator createBlogEntryCoordinator, IFindBlogArticleCoordinator findBlogArticleCoordinator)
        {
            _createBlogEntryCoordinator = createBlogEntryCoordinator;
            _findBlogArticleCoordinator = findBlogArticleCoordinator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody]ArticleRequest article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            var result = await _createBlogEntryCoordinator.CreateNewArticleAsync(article);

            if (result == null)
            {
                return BadRequest();
            }

            var response = MapArticleResponse(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetArticle(Guid id)
        {
            var result = await _findBlogArticleCoordinator.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            var response = MapArticleResponse(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/entries")]
        public async Task<IActionResult> GetArticleEntryCollection()
        {
            return Ok();
        }

        private ArticleResponse MapArticleResponse(Article result)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.SELF, Hred = Url.Action(nameof(GetArticle), new { id = result.Id})}, 
                new Link {Relation = LinkRelValueObject.GET_ENTRY_COLLECTION, Hred = Url.Action(nameof(GetArticleEntryCollection), new { id = result.Id})}, 
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