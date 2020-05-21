using Bog.Api.Common.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateArticleCoordinator : ICreateArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;
        private readonly IClock _clock;

        public CreateArticleCoordinator(IBlogApiDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
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
                || string.IsNullOrWhiteSpace(request.Author)
                || string.IsNullOrWhiteSpace(request.Title))
            {
                return null;
            }

            var newBlogArticle = new Article()
            {
                BlogId = blog.Id,
                Author = request.Author,
                Title = request.Title,
                Description = request.Description,
                Created = _clock.Now
            };

            await _context.Add(newBlogArticle);
            await _context.SaveChanges();

            return newBlogArticle;
        }

        private async Task<Blog> GetBlogForEntry(Guid newEntryBlogId) => await _context.Find<Blog>(newEntryBlogId);
    }
}