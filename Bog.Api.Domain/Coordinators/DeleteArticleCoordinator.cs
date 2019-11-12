using System;
using System.Threading.Tasks;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class DeleteArticleCoordinator : IDeleteArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;
        private readonly IClock _clock;

        public DeleteArticleCoordinator(IBlogApiDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task<bool> TryDeleteArticle(Guid articleId)
        {
            var articleToDelete = await _context.Find<Article>(articleId);

            if (articleToDelete == null)
            {
                return false;
            }

            if (articleToDelete.IsDeleted)
            {
                return true;
            }

            articleToDelete.IsDeleted = true;
            articleToDelete.Deleted = _clock.Now;
            
            await _context.SaveChanges();

            return true;
        }
    }
}