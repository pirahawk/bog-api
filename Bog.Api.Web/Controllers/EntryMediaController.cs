using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Microsoft.Net.Http.Headers;

namespace Bog.Api.Web.Controllers
{
    [Route("api/media")]
    public class EntryMediaController : Controller
    {
        private readonly ICreateAndPersistArticleEntryMediaStrategy _createStrategy;

        public EntryMediaController(ICreateAndPersistArticleEntryMediaStrategy createStrategy)
        {
            _createStrategy = createStrategy;
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

    }
}