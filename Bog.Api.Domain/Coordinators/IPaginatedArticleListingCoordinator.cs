using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Coordinators
{
    public interface IPaginatedArticleListingCoordinator
    {
        Task<IEnumerable<Article>> Find(Guid blogId, int? skip, int? take, string[] filter, string[] include);
    }
}