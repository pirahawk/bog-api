using System.Linq;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Db.DbContexts
{
    public class BlogApiDbContextAdapter: IBlogApiDbContext
    {
        private readonly BlogApiDbContext _context;

        public IQueryable<Blog> Blogs => _context.Blogs;
        public IQueryable<Article> Articles => _context.Articles;

        public BlogApiDbContextAdapter(BlogApiDbContext context)
        {
            _context = context;
        }

    }
}