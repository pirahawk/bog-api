﻿using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Bog.Api.Web.Configuration.Filters;

namespace Bog.Api.Web.Controllers
{
    [ApiController]
    [Route("api/article")]
    [ServiceFilter(typeof(ApiKeyAuthenticationFilterAttribute))]
    public class ArticleController : ControllerBase
    {
        private readonly ICreateArticleCoordinator _createArticleCoordinator;
        private readonly IFindBlogArticleCoordinator _findBlogArticleCoordinator;
        private readonly IUpdateArticleCoordinator _updateArticleCoordinator;
        private readonly IDeleteArticleCoordinator _deleteArticleCoordinator;

        public ArticleController(ICreateArticleCoordinator createArticleCoordinator, 
            IFindBlogArticleCoordinator findBlogArticleCoordinator, 
            IUpdateArticleCoordinator updateArticleCoordinator, 
            IDeleteArticleCoordinator deleteArticleCoordinator)
        {
            _createArticleCoordinator = createArticleCoordinator;
            _findBlogArticleCoordinator = findBlogArticleCoordinator;
            _updateArticleCoordinator = updateArticleCoordinator;
            _deleteArticleCoordinator = deleteArticleCoordinator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody]ArticleRequest article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            var result = await _createArticleCoordinator.CreateNewArticleAsync(article);

            if (result == null)
            {
                return BadRequest();
            }

            var response = MapArticleResponse(result);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateArticle(Guid id, [FromBody] ArticleRequest article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            var result = await _updateArticleCoordinator.TryUpdateArticle(id ,article);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetArticle(Guid id)
        {
            var result = await _findBlogArticleCoordinator.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            var response = MapArticleResponse(result);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var result = await _deleteArticleCoordinator.TryDeleteArticle(id);
            return result ? (IActionResult) NoContent() : BadRequest();
        }

        private ArticleResponse MapArticleResponse(Article result)
        {
            var links = new Link[]
            {
                new Link {Relation = LinkRelValueObject.SELF, Href = Url.Action(nameof(GetArticle), new { id = result.Id})},
                new Link {Relation = LinkRelValueObject.ENTRY, Href = Url.Action("GetLatestArticleEntry","Entries", new { articleId = result.Id})},
                new Link {Relation = LinkRelValueObject.MEDIA_LOOKUP, Href = Url.Action("GetMediaContentLookup","EntryMedia", new { articleId = result.Id})},
                new Link {Relation = LinkRelValueObject.META_TAG, Href = Url.Action("AddMetaTag", "MetaTag", new {articleId = result.Id})},
            };

            return new ArticleResponse
            {
                Id = result.Id,
                BlogId = result.BlogId,
                Author = result.Author,
                Title = result.Title,
                Description = result.Description,
                IsPublished = result.IsPublished,
                Updated = result.Updated,
                IsDeleted = result.IsDeleted,
                Deleted = result.Deleted,
                Links = links
            };
        }
    }
}