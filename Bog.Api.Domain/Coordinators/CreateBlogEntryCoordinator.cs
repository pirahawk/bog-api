using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateBlogEntryCoordinator : ICreateBlogEntryCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public CreateBlogEntryCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }
        public async Task<Article> CreateNewEntryAsync(NewEntryRequest newEntry)
        {
            if (newEntry == null) throw new ArgumentNullException(nameof(newEntry));

            var blog = GetBlogForEntry(newEntry.BlogId);

            if (blog == null
                || string.IsNullOrWhiteSpace(newEntry.Author))
            {
                return null;
            }

            var newBlogArticle = new Article()
            {
                BlogId =  blog.Id,
                Created = DateTimeOffset.UtcNow
            };

            await _context.Add(newBlogArticle);
            await _context.SaveChanges();

            return null;
        }

        private Blog GetBlogForEntry(Guid newEntryBlogId) => _context.Blogs.FirstOrDefault(b => b.Id == newEntryBlogId);
    }
}