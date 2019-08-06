using Bog.Api.Domain.DbContext;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public void Delete<TEntity>(params TEntity[] entities) where TEntity : class
        {
            Delete(entities.AsEnumerable());
        }

        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.RemoveRange(entities);
        }

        public IQueryable<TEntity> Query<TEntity>(params string[] includes) where TEntity : class
        {
            var queryable = _context.Get<TEntity>();

            if (queryable != null && includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    queryable = queryable.Include(include);
                }
            }

            return queryable;
        } 
    }
}