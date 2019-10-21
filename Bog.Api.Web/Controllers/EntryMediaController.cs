using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Web.Controllers
{
    [Route("api/media")]
    public class EntryMediaController : Controller
    {
        private readonly ICreateAndPersistArticleEntryMediaStrategy _createStrategy;
        private readonly IEntryMediaSearchStrategy _mediaSearchStrategy;

        public EntryMediaController(ICreateAndPersistArticleEntryMediaStrategy createStrategy, IEntryMediaSearchStrategy mediaSearchStrategy)
        {
            _createStrategy = createStrategy;
            _mediaSearchStrategy = mediaSearchStrategy;
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

            return Ok();
        }

        [Route("{entryId:guid}")]
        [HttpHead]
        public async Task<IActionResult> FindEntryMedia(Guid entryId, [FromHeader(Name = "If-Match")] string ifMatch)
        {
            if (string.IsNullOrWhiteSpace(ifMatch))
            {
                return NotFound();
            }
            
            var result = await _mediaSearchStrategy.Find(entryId, ifMatch);

            if (result == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}