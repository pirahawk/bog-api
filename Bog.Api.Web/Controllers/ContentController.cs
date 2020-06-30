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
    [Route("api/content")]
    public class ContentController : ControllerBase
    {
        private readonly IFindBlogArticleCoordinator _findBlogArticleCoordinator;
        private readonly IBogMarkdownConverterStrategy _bogMarkdownConverterStrategy;

        public ContentController(IFindBlogArticleCoordinator findBlogArticleCoordinator, IBogMarkdownConverterStrategy bogMarkdownConverterStrategy)
        {
            _findBlogArticleCoordinator = findBlogArticleCoordinator;
            _bogMarkdownConverterStrategy = bogMarkdownConverterStrategy;
        }

        [HttpGet]
        [Route("{articleId:guid}")]
        public async Task<IActionResult> GetArticle(Guid articleId)
        {
            var article = await _findBlogArticleCoordinator.Find(articleId);

            if (article == null)
            {
                return NotFound();
            }

            var latestEntryContentLink = await _bogMarkdownConverterStrategy.GetLatestConvertedEntryContentUri(articleId);
            var result = MapContentResponse(article, latestEntryContentLink, string.Empty);
            return Ok(result);
        }

        private ContentResponse MapContentResponse(Article article, string latestEntryContentLink, string keywords)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.CONTENT_BLOB_URL, Href = latestEntryContentLink},
            };


            //TODO Add Tags
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