using System.Linq;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.DbContext
{
    public interface IBlogApiDbContext
    {
        IQueryable<Blog> Blogs { get; }
    }
}