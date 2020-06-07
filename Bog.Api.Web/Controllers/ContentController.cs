using Bog.Api.Common;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Markdown;
using Bog.Api.Domain.Models.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bog.Api.Web.Controllers
{
    [ApiController]
    [Route("api/content")]
    public class ContentController : ControllerBase
    {
        private readonly IFindBlogArticleCoordinator _findBlogArticleCoordinator;
        private readonly IBogMarkdownConverterStrategy _bogMarkdownConverterStrategy;

        public ContentController(IFindBlogArticleCoordinator findBlogArticleCoordinator, 
            IBogMarkdownConverter mdConverter, IBogMarkdownConverterStrategy bogMarkdownConverterStrategy)
        {
            _findBlogArticleCoordinator = findBlogArticleCoordinator;
            _bogMarkdownConverterStrategy = bogMarkdownConverterStrategy;
        }

        [HttpGet]
        [Route("{articleId:guid}")]
        public async Task<IActionResult> GetArticle(Guid articleId)
        {
            var findArticleTask = _findBlogArticleCoordinator.Find(articleId);
            var latestConvertedEntryContentUriTask = _bogMarkdownConverterStrategy.GetLatestConvertedEntryContentUri(articleId);

            var article = await findArticleTask;

            if (article == null)
            {
                return NotFound();
            }

            var latestEntryContentLink = await latestConvertedEntryContentUriTask;
            var result = MapContentResponse(article, latestEntryContentLink, string.Empty);
            return Ok(result);
        }

        private ContentResponse MapContentResponse(Article article, string latestEntryContentLink, string keywords)
        {
            //TODO Add entry content link to LINKS when ready
            Link[] links = Enumerable.Empty<Link>().ToArray();

            var mapContentResponse = new ContentResponse
            {
                Author = article.Author,
                Title = article.Title,
                Description = article.Description,

                IsPublished = article.IsPublished,
                Updated = article.Updated,
                Deleted = article.Deleted,
                IsDeleted = article.IsDeleted,

                KeyWords = keywords,
                Links = links
            };

            return mapContentResponse;
        }
    }
}