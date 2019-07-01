using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.DbContext
{
    public interface IBlogApiDbContext
    {
        IQueryable<Blog> Blogs { get; }
        IQueryable<Article> Articles { get; }

        Task<int> SaveChanges();

        Task Add<TEntity>(params TEntity[] newEntities);
        Task<TEntity> Find<TEntity>(params object[] keyValues) where TEntity : class;
    }
}