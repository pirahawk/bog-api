using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddMetaTag(Guid articleId, [FromBody]MetaTagRequest metaTagRequest)
        {
            if (string.IsNullOrWhiteSpace(metaTagRequest?.Name))
            {
                return BadRequest("Missing meta-tag name");
            }

            var result = await _addMetaTagCoordinator.AddArticleMetaTag(articleId,metaTagRequest);

            if (result == null)
            {
                return BadRequest("could not create metatag");
            }

            return NoContent();
        }
    }
}