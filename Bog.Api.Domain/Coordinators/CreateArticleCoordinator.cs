using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateArticleCoordinator : ICreateArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public CreateArticleCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }
        public async Task<Article> CreateNewArticleAsync(ArticleRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return await AttemptCreateNewBlogArticle(request);
        }

        private async Task<Article> AttemptCreateNewBlogArticle(ArticleRequest request)
        {
            var blog = await GetBlogForEntry(request.BlogId);

            if (blog == null
                || string.IsNullOrWhiteSpace(request.Author))
            {
                return null;
            }

            var newBlogArticle = new Article()
            {
                BlogId = blog.Id,
                Author = request.Author,
                Created = DateTimeOffset.UtcNow
            };

            await _context.Add(newBlogArticle);
            await _context.SaveChanges();

            return newBlogArticle;
        }

        private async Task<Blog> GetBlogForEntry(Guid newEntryBlogId) => await _context.Find<Blog>(newEntryBlogId);
    }
}