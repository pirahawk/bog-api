using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task Add<TEntity>(params TEntity[] newEntities)
        {
            if (newEntities == null) throw new ArgumentNullException(nameof(newEntities));

            foreach (var newEntity in newEntities)
            {
                await _context.AddAsync(newEntity);
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}