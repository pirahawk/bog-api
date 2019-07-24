using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Web.Controllers
{
    [Route("api/entry")]
    public class EntriesController: Controller
    {
        private readonly BlogApiSettings _apiSettings;
        private readonly ICreateAndPersistArticleEntryStrategy _persistArticleEntryStrategy;

        public EntriesController(ICreateAndPersistArticleEntryStrategy persistArticleEntryStrategy)
        {
            _persistArticleEntryStrategy = persistArticleEntryStrategy;
        }

        [HttpPost]
        [Route("{articleId:guid}")]
        [RequestSizeLimit(BlogApiSettings.MAX_ENTRY_REQUEST_LIMIT_BYTES)]
        public async Task<IActionResult> AddArticleEntry(Guid articleId, [FromBody]ArticleEntry post)
        {
            var result = await _persistArticleEntryStrategy.PersistArticleEntryAsync(articleId, post);

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
                Links = links,
            };
        }

    }
}