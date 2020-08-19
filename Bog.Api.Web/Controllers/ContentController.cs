using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IGetTagsForArticleCoordinator _getTagsForArticleCoordinator;
        private readonly IPaginatedArticleListingCoordinator _paginatedArticleListingCoordinator;

        public ContentController(IFindBlogArticleCoordinator findBlogArticleCoordinator, IBogMarkdownConverterStrategy bogMarkdownConverterStrategy, IGetTagsForArticleCoordinator getTagsForArticleCoordinator, IPaginatedArticleListingCoordinator paginatedArticleListingCoordinator)
        {
            _findBlogArticleCoordinator = findBlogArticleCoordinator;
            _bogMarkdownConverterStrategy = bogMarkdownConverterStrategy;
            _getTagsForArticleCoordinator = getTagsForArticleCoordinator;
            _paginatedArticleListingCoordinator = paginatedArticleListingCoordinator;
        }

        [HttpGet]
        [Route("article/{articleId:guid}")]
        public async Task<IActionResult> GetArticle(Guid articleId)
        {
            var article = await _findBlogArticleCoordinator.Find(articleId);

            if (article == null)
            {
                return NotFound();
            }

            var latestEntryContentLink = await _bogMarkdownConverterStrategy.GetLatestConvertedEntryContentUri(articleId);
            var metaTags = _getTagsForArticleCoordinator.GetAllTagsForArticle(articleId);
            var result = MapContentResponse(article, latestEntryContentLink, metaTags);
            return Ok(result);
        }

        [HttpGet]
        [Route("{blogId:guid}")]
        public async Task<IActionResult> GetArticles([FromRoute] Guid blogId,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] string filter,
            [FromQuery] string include)
        {
            var excludeFilters = filter?.Split('|');
            var includeFilters = include?.Split('|');
            var searchResults = await _paginatedArticleListingCoordinator
                .Find(blogId, skip, take, excludeFilters, includeFilters);
            var searchArticles = searchResults
                .Select(article => MapContentResponse(article, string.Empty, article.MetaTags.Select(mt => mt.Name)));
            return Ok(searchArticles);
        }

        private ContentResponse MapContentResponse(Article article, string latestEntryContentLink, IEnumerable<string> metaTags)
        {
            var links = new List<Link>()
            {
                new Link { Relation = LinkRelValueObject.SELF, Href = Url.Action(nameof(GetArticle), new { articleId = article.Id}) }
            };

            if (!string.IsNullOrWhiteSpace(latestEntryContentLink))
            {
                links.Add(new Link { Relation = LinkRelValueObject.CONTENT_BLOB_URL, Href = latestEntryContentLink });
            }

            var mapContentResponse = new ContentResponse
            {
                Id = article.Id,
                Author = article.Author,
                Title = article.Title,
                Description = article.Description,
                Created = article.Created,
                IsPublished = article.IsPublished,
                Updated = article.Updated,
                Deleted = article.Deleted,
                IsDeleted = article.IsDeleted,
                KeyWords = metaTags.ToArray(),
                Links = links.ToArray()
            };

            return mapContentResponse;
        }
    }
}