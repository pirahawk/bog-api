using System;
using System.Threading.Tasks;
using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class UploadArticleEntryMediaCoordinator : IUploadArticleEntryMediaCoordinator
    {
        private readonly IBlobStore _blobStore;

        public UploadArticleEntryMediaCoordinator(IBlobStore blobStore)
        {
            _blobStore = blobStore;
        }

        public async Task<string> UploadEntryMedia(ArticleEntryMediaRequest entryMediaRequest,
            EntryMedia articleEntryMedia)
        {
            if (entryMediaRequest == null) throw new ArgumentNullException(nameof(entryMediaRequest));
            if (articleEntryMedia == null) throw new ArgumentNullException(nameof(articleEntryMedia));
            if (entryMediaRequest.MediaContent == null) throw new Exception($"{nameof(entryMediaRequest.MediaContent)} cannot be null");

            var uploadUri = await _blobStore.PersistArticleEntryMedia( articleEntryMedia.Id, 
                articleEntryMedia.EntryContentId, 
                entryMediaRequest.MediaContent, 
                entryMediaRequest.ContentType);

            return uploadUri;
        }
    }
}