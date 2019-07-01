using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public class FindBlogArticleCoordinator : IFindBlogArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public FindBlogArticleCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<Article> Find(Guid articleId)
        {
            return await _context.Find<Article>(articleId);
        }
    }
}