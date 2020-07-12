using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Web.Configuration.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Bog.Api.Web.Controllers
{
    [ApiController]
    [Route("api/meta")]
    [ServiceFilter(typeof(ApiKeyAuthenticationFilterAttribute))]
    public class MetaTagController : ControllerBase
    {
        private IAddMetaTagForArticleCoordinator _addMetaTagCoordinator;
        private IRemoveMetaTagForArticleCoordinator _removeMetaTagCoordinator;

        public MetaTagController(IAddMetaTagForArticleCoordinator addMetaTagCoordinator, IRemoveMetaTagForArticleCoordinator removeMetaTagCoordinator)
        {
            _addMetaTagCoordinator = addMetaTagCoordinator;
            _removeMetaTagCoordinator = removeMetaTagCoordinator;
        }

        [HttpPost]
        [Route("{articleId:guid}")]
        public async Task<IActionResult> AddMetaTag(Guid articleId, [FromBody]MetaTagRequest[] tagsToAdd)
        {
            if (tagsToAdd == null) return BadRequest("No meta-tags included");

            var result = await _addMetaTagCoordinator.AddArticleMetaTags(articleId, tagsToAdd);

            if (result == null || !result.Any())
            {
                return BadRequest("could not create meta-tags");
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("{articleId:guid}")]
        public async Task<IActionResult> RemoveTags(Guid articleId, [FromBody]MetaTagRequest[] tagsToDelete)
        {
            if (tagsToDelete == null) return BadRequest("No meta-tags included");

            await _removeMetaTagCoordinator.RemoveArticleMetaTags(articleId, tagsToDelete);
            return NoContent();
        }
    }
}