using System.Linq;
using System.Threading.Tasks;

namespace Bog.Api.Domain.DbContext
{
    public interface IBlogApiDbContext
    {
        Task<int> SaveChanges();
        Task Add<TEntity>(params TEntity[] newEntities);
        Task<TEntity> Find<TEntity>(params object[] keyValues) where TEntity : class;
        void Delete<TEntity>(params TEntity[] entities) where TEntity : class;
        IQueryable<TEntity> Query<TEntity>(params string[] includes) where TEntity : class;
    }
}