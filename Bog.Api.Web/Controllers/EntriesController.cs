using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Web.Controllers
{
    [Route("api/entry")]
    public class EntriesController: Controller
    {
        [HttpPost]
        [Route("{articleId:guid}")]
        [RequestSizeLimit(20971520)] // 20 MB limit
        public async Task<IActionResult> AddArticleEntry(Guid articleId, [FromBody]ArticleEntry post)
        {
            return Ok();
        }

    }
}