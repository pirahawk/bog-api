using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Values;

namespace Bog.Api.Domain.BlobStore
{
    public interface IBlobStore
    {
        Task<bool> TryCreateContainer(BlobStorageContainer container);
        Task<string> PersistArticleEntryAsync(BlobStorageContainer container, Guid articleId, Guid entryContentId, string contentBase64);
        Task<string> PersistArticleEntryMedia(Guid entryMediaId, Guid entryContentId, byte[] mediaContent, string contentType);
    }
}