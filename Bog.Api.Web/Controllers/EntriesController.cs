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
    [ApiController]
    [Route("api/entry")]
    public class EntriesController: ControllerBase
    {
        private readonly ICreateAndPersistArticleEntryStrategy _persistArticleEntryStrategy;
        private readonly IGetLatestArticleEntryStrategy _getLatestArticleEntryStrategy;

        public EntriesController(ICreateAndPersistArticleEntryStrategy persistArticleEntryStrategy, IGetLatestArticleEntryStrategy getLatestArticleEntryStrategy)
        {
            _persistArticleEntryStrategy = persistArticleEntryStrategy;
            _getLatestArticleEntryStrategy = getLatestArticleEntryStrategy;
        }

        [Route("{articleId:guid}")]
        [HttpPost]
        [RequestSizeLimit(BlogApiSettings.MAX_ENTRY_REQUEST_LIMIT_BYTES)]
        public async Task<IActionResult> AddArticleEntry(Guid articleId, [FromBody]ArticleEntry post)
        {
            var result = await _persistArticleEntryStrategy.PersistArticleEntryAsync(articleId, post);

            if (result == null)
            {
                return BadRequest();
            }

            var response = MapEntryResponse(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("{articleId:guid}")]
        public async Task<IActionResult> GetLatestArticleEntry(Guid articleId)
        {
            var result = await _getLatestArticleEntryStrategy.FindLatestEntry(articleId);

            if (result == null)
            {
                return NotFound();
            }

            var response = MapEntryResponse(result);
            return Ok(response);
        }

        private ArticleEntryResponse MapEntryResponse(EntryContent result)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.ARTICLE, Href = Url.Action("GetArticle", "Article", new { id = result.ArticleId})},
                new Link {Relation = LinkRelValueObject.SELF, Href = Url.Action(nameof(GetLatestArticleEntry), new { articleId = result.ArticleId})},
                new Link {Relation = LinkRelValueObject.MEDIA, Href = Url.Action("UploadMediaContent", "EntryMedia", new { entryId = result.Id})},
            };

            return new ArticleEntryResponse
            {
                Id = result.Id,
                ArticleId = result.ArticleId,
                Created = result.Created,
                Persisted = result.Persisted,
                Links = links,
            };
        }
    }
}