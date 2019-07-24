using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Values;

namespace Bog.Api.Domain.BlobStore
{
    public interface IBlobStore
    {
        Task<bool> TryCreateContainer(BlobStorageContainer container);
        Task PersistArticleEntryAsync(BlobStorageContainer container, Guid articleId, Guid entryContentId, string contentBase64);
    }
}