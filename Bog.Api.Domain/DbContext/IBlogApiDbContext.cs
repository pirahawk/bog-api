using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.DbContext
{
    public interface IBlogApiDbContext
    {
        IQueryable<Blog> Blogs { get; }
        IQueryable<Article> Articles { get; }

        Task Add<TEntity>(params TEntity[] newEntities);
        Task<int> SaveChanges();
    }
}