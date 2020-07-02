using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public interface IAddMetaTagForArticleCoordinator
    {
        Task<MetaTag[]> AddArticleMetaTags(Guid articleId, params MetaTagRequest[] metaTagRequests);
    }
}