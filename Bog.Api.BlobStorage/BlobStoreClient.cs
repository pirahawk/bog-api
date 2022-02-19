using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Values;
using System;
using System.Threading.Tasks;

namespace Bog.Api.BlobStorage
{
    public class BlobStoreClient : IBlobStore
    {
        public Task<string> PersistArticleEntryAsync(BlobStorageContainer container, Guid articleId, Guid entryContentId, string contentBase64)
        {
            throw new NotImplementedException();
        }

        public Task<string> PersistArticleEntryMedia(Guid entryMediaId, Guid entryContentId, byte[] mediaContent, string contentType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryCreateContainer(BlobStorageContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
