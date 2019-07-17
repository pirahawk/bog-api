using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Values;

namespace Bog.Api.Web.Controllers
{
    [Route("api/entry")]
    public class EntriesController: Controller
    {
        private readonly ICreateArticleEntryCoordinator _createEntryCoordinator;

        public EntriesController(ICreateArticleEntryCoordinator createEntryCoordinator)
        {
            _createEntryCoordinator = createEntryCoordinator;
        }

        [HttpPost]
        [Route("{articleId:guid}")]
        [RequestSizeLimit(20971520)] // 20 MB limit
        public async Task<IActionResult> AddArticleEntry(Guid articleId, [FromBody]ArticleEntry post)
        {
            var result = await _createEntryCoordinator.CreateArticleEntry(articleId, post);

            if (result == null)
            {
                return BadRequest();
            }

            var response = MapArticleResponse(result);
            return Ok(response);
        }

        private ArticleEntryResponse MapArticleResponse(EntryContent result)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.ARTICLE, Href = Url.Action("GetArticle", "Article", new { id = result.ArticleId})},
            };

            return new ArticleEntryResponse
            {
                Id = result.Id,
                ArticleId = result.ArticleId,
                Created = result.Created,
                Deleted = result.Deleted,
                Updated = result.Updated,
                Links = links,
            };
        }

    }
}