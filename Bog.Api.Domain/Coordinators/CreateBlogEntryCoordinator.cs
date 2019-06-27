using System;
using System.Linq;
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
        public Article CreateNewEntry(NewEntryRequest newEntry)
        {
            if (newEntry == null) throw new ArgumentNullException(nameof(newEntry));

            var blog = GetBlogForEntry(newEntry.BlogId);

            if (blog == null)
            {
                return null;
            }

            var newBlogArticle = new Article()
            {
                BlogId =  blog.Id
            };


            return null;
        }

        private Blog GetBlogForEntry(Guid newEntryBlogId) => _context.Blogs.FirstOrDefault(b => b.Id == newEntryBlogId);
    }
}