using System;
using System.Linq;
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

        public async Task<MetaTag[]> AddArticleMetaTags(Guid articleId, params MetaTagRequest[] metaTagRequests)
        {
            if (metaTagRequests == null) throw new ArgumentNullException(nameof(metaTagRequests));
            var existingArticle = GetExistingArticle(articleId);

            if (existingArticle == null)
            {
                return null;
            }

            var tagsToCreate = metaTagRequests
                .Where(mtr => existingArticle.MetaTags.All(mt => mt.Name != mtr.Name))
                .ToArray();

            if (!tagsToCreate.Any())
            {
                return null;
            }

            var newMetaTagsForArticle = tagsToCreate.Select(nt => new MetaTag
            {
                ArticleId = existingArticle.Id,
                Article = existingArticle,
                Name = nt.Name
            }).ToArray();

            await _context.Add(newMetaTagsForArticle);
            await _context.SaveChanges();

            return newMetaTagsForArticle;
        }

        private Article GetExistingArticle(Guid articleId)
        {
            var existingArticle = _context.Query<Article>("MetaTags")
                .FirstOrDefault(article => article.Id == articleId);
            return existingArticle;
        }
    }
}