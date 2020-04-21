using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Article;
using Bog.Api.Domain.Values;

namespace Bog.Api.Web.Controllers
{
    [ApiController]
    [Route("api/media")]
    public class EntryMediaController : ControllerBase
    {
        private readonly ICreateAndPersistArticleEntryMediaStrategy _createStrategy;
        private readonly IEntryMediaSearchStrategy _mediaSearchStrategy;
        private readonly IGetArticleMediaLookupStrategy _getArticleMediaLookupStrategy;

        public EntryMediaController(ICreateAndPersistArticleEntryMediaStrategy createStrategy, IEntryMediaSearchStrategy mediaSearchStrategy, IGetArticleMediaLookupStrategy getArticleMediaLookupStrategy)
        {
            _createStrategy = createStrategy;
            _mediaSearchStrategy = mediaSearchStrategy;
            _getArticleMediaLookupStrategy = getArticleMediaLookupStrategy;
        }

        [Route("{articleId:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetMediaContentLookup(Guid articleId)
        {
            var articleLookup = await _getArticleMediaLookupStrategy.GetMediaLookup(articleId);

            if (articleLookup == null)
            {
                return NotFound();
            }

            var response = MapMediaResponse(articleId, articleLookup);
            return Ok(response);
        }

        [Route("{entryId:guid}")]
        [HttpPost]
        public async Task<IActionResult> UploadMediaContent(Guid entryId, [FromBody] ArticleEntryMediaRequest media)
        {
            if (media == null)
            {
                return BadRequest("Could not establish article media from request");
            }

            if (string.IsNullOrWhiteSpace(media.FileName))
            {
                return BadRequest($"Could not parse filename from {HeaderNames.ContentDisposition} header");
            }

            if (string.IsNullOrWhiteSpace(media.ContentType))
            {
                return BadRequest($"Could not parse content-type from {HeaderNames.ContentType} header");
            }

            media.EntryId = entryId;

            var entryMedia = await _createStrategy.PersistArticleEntryMediaAsync(media);

            if (entryMedia == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("{entryId:guid}")]
        [HttpHead]
        public async Task<IActionResult> FindEntryMedia(Guid entryId, 
            [FromHeader(Name = "If-Match")] string ifMatch, 
            [FromHeader(Name = "Content-Disposition")] string contentDisposition)
        {
            if (string.IsNullOrWhiteSpace(ifMatch))
            {
                return NotFound();
            }

            ifMatch = ifMatch.Replace("\"", string.Empty);
            string fileName = HeaderUtilityHelper.TryGetContentDispositionFileName(contentDisposition);
            EntryMedia result = string.IsNullOrWhiteSpace(fileName)
                ? await _mediaSearchStrategy.Find(entryId, ifMatch)
                : await _mediaSearchStrategy.Find(entryId, ifMatch, fileName);


            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        private ArticleMediaLookupResponse MapMediaResponse(Guid articleId, ArticleMediaLookup articleLookup)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.ARTICLE, Href = Url.Action("GetArticle", "Article", new { id = articleId})},
            };

            var lookup = articleLookup?.MediaLookup ?? new Dictionary<string, string>();
            return new ArticleMediaLookupResponse
            {
                ArticleId = articleId,
                MediaLookup = lookup,
                Links = links
            };
        }
    }
}