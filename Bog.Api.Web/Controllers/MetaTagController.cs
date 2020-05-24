using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Bog.Api.Web.Controllers
{
    [ApiController]
    [Route("api/meta")]
    public class MetaTagController : ControllerBase
    {
        private IAddMetaTagForArticleCoordinator _addMetaTagCoordinator;

        public MetaTagController(IAddMetaTagForArticleCoordinator addMetaTagCoordinator)
        {
            _addMetaTagCoordinator = addMetaTagCoordinator;
        }

        [Route("{articleId:guid}")]
        public async Task<IActionResult> AddMetaTag(Guid articleId, [FromBody]MetaTagRequest[] metaTagRequests)
        {
            if (metaTagRequests == null)
            {
                return BadRequest("No meta-tags included");
            }

            var result = await _addMetaTagCoordinator.AddArticleMetaTags(articleId, metaTagRequests);

            if (result == null || !result.Any())
            {
                return BadRequest("could not create meta-tags");
            }

            return NoContent();
        }
    }
}