using Bog.Api.Domain.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public interface IArticleSearchListingCoordinator
    {
        Task<IQueryable<Article>> Find(Guid blogId);
    }
}