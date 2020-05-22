using System;
using System.Threading.Tasks;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class AddMetaTagForArticleCoordinator : IAddMetaTagForArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;
        public AddMetaTagForArticleCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<MetaTag> AddArticleMetaTag(Guid articleId, MetaTagRequest metaTagRequest)
        {
            if (metaTagRequest == null) throw new ArgumentNullException(nameof(metaTagRequest));
            if (string.IsNullOrWhiteSpace(metaTagRequest.Name)) throw new ArgumentNullException(nameof(metaTagRequest.Name));

            var existingArticle = await _context.Find<Article>(articleId);

            if (existingArticle == null)
            {
                return null;
            }

            var metaTag = new MetaTag
            {
                ArticleId = existingArticle.Id,
                Article = existingArticle,
                Name = metaTagRequest.Name
            };

            await _context.Add(metaTag);
            await _context.SaveChanges();

            return metaTag;
        }
    }
}