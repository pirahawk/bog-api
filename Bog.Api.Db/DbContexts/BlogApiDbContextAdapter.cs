using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bog.Api.Db.DbContexts
{
    public class BlogApiDbContextAdapter: IBlogApiDbContext
    {
        private readonly BlogApiDbContext _context;

        public BlogApiDbContextAdapter(BlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task Add<TEntity>(params TEntity[] newEntities)
        {
            if (newEntities == null) throw new ArgumentNullException(nameof(newEntities));

            foreach (var newEntity in newEntities)
            {
                await _context.AddAsync(newEntity);
            }
        }

        public async Task<TEntity> Find<TEntity>(params object[] keyValues) where TEntity:class
        {
            return await _context.FindAsync<TEntity>(keyValues);
        }


    }
}