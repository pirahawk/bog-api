using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class ActiveArticleSearchListingCoordinator : IArticleSearchListingCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public ActiveArticleSearchListingCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Article>> Find(Guid blogId)
        {
            var allBlogsQuery = _context.Query<Blog>();
            var allBlogArticlesQuery = FilterArticlesForBlog(blogId, allBlogsQuery);
            var allActiveArticlesQuery = FilterActiveArticles(allBlogArticlesQuery);
            return await Task.FromResult(allActiveArticlesQuery);
        }

        private static IQueryable<Article> FilterArticlesForBlog(Guid blogId, IQueryable<Blog> allBlogsQuery)
        {
            return allBlogsQuery
                .Where(blog => blog.Id == blogId)
                .SelectMany(bl => bl.Articles);
        }

        private IQueryable<Article> FilterActiveArticles(IQueryable<Article> query)
        {
            return query.Where(a => a.IsPublished && !a.IsDeleted);
        }
    }
}