using Bog.Api.Common;
using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public class UploadArticleEntryCoordinator : IUploadArticleEntryCoordinator
    {
        private readonly IBlobStore _blobstore;

        public UploadArticleEntryCoordinator(IBlobStore blobstore)
        {
            _blobstore = blobstore;
        }
        public async Task<string> UploadArticleEntry(EntryContent entryContent, ArticleEntry articleEntry)
        {
            if (entryContent == null) throw new ArgumentNullException(nameof(entryContent));
            if (articleEntry == null) throw new ArgumentNullException(nameof(articleEntry));

            if (string.IsNullOrWhiteSpace(articleEntry.Content))
            {
                return null;
            }

            var contentBase64 = StringUtilities.ToBase64(articleEntry.Content);
            return await _blobstore.PersistArticleEntryAsync(BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER, entryContent.ArticleId, entryContent.Id, contentBase64);
        }
    }
}