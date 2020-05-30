using Bog.Api.Domain.Models.Http;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public interface IRemoveMetaTagForArticleCoordinator
    {
        Task RemoveArticleMetaTags(Guid articleId, params MetaTagRequest[] metaTagRequests);
    }
}