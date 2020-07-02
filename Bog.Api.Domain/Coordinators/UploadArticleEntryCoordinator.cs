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

        public async Task<string> UploadMarkdownArticleEntry(EntryContent entryContent, ArticleEntry articleEntry)
        {
            if (articleEntry == null) throw new ArgumentNullException(nameof(articleEntry));

            if (string.IsNullOrWhiteSpace(articleEntry.Content))
            {
                return null;
            }

            return await UploadArticleEntry(entryContent, articleEntry.Content, BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER);
        }

        public async Task<string> UploadConvertedArticleEntry(EntryContent entryContent, string convertedContent)
        {
            if (string.IsNullOrWhiteSpace(convertedContent))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(convertedContent));

            return await UploadArticleEntry(entryContent, convertedContent, BlobStorageContainer.TRANSLATED_ARTICLE_ENTRIES_CONTAINER);
        }

        private async Task<string> UploadArticleEntry(EntryContent entryContent, string content, BlobStorageContainer storageContainer)
        {
            if (entryContent == null) throw new ArgumentNullException(nameof(entryContent));

            var contentBase64 = StringUtilities.ToBase64(content);
            return await _blobstore.PersistArticleEntryAsync(storageContainer, entryContent.ArticleId, entryContent.Id, contentBase64);
        }
    }
}