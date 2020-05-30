using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class RemoveMetaTagForArticleCoordinator : IRemoveMetaTagForArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public RemoveMetaTagForArticleCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task RemoveArticleMetaTags(Guid articleId, params MetaTagRequest[] metaTagRequests)
        {
            if (metaTagRequests == null || !metaTagRequests.Any()) throw new ArgumentNullException(nameof(metaTagRequests));

            var existingArticle = GetExistingArticle(articleId);

            if (existingArticle == null)
            {
                return;
            }

            var tagsToDelete = existingArticle.MetaTags
                .Where(mt => metaTagRequests.Any(mtr => mtr.Name == mt.Name));

            foreach (var tagToDelete in tagsToDelete)
            {
                _context.Delete(tagToDelete);
            }

            await _context.SaveChanges();
        }

        private Article GetExistingArticle(Guid articleId)
        {
            var existingArticle = _context.Query<Article>("MetaTags")
                .FirstOrDefault(article => article.Id == articleId);
            return existingArticle;
        }
    }
}