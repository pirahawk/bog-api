using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Bog.Api.Web.Controllers
{
    [Route("api/media")]
    public class EntryMediaController : Controller
    {
        [Route("{entryId:guid}")]
        [HttpPost]
        public async Task<IActionResult> UploadMediaContent(Guid entryId, [FromBody] ArticleEntryMediaRequest media)
        {
            if (media == null)
            {
                return BadRequest("Could not establish article media from request");
            }

            if (media.FileName == null)
            {
                return BadRequest($"Could not parse filename from {HeaderNames.ContentDisposition} header");
            }

            media.EntryId = entryId;

            
            return Ok();
        }

    }
}