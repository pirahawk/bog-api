using System;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategy : ICreateAndPersistArticleEntryMediaStrategy
    {
        private readonly ICreateEntryMediaCoordinator _createEntryMediaCoordinator;
        private readonly IUploadArticleEntryMediaCoordinator _uploadCoordinator;
        private readonly IEntryMediaSearchStrategy _searchStrategy;

        public CreateAndPersistArticleEntryMediaStrategy(ICreateEntryMediaCoordinator createEntryMediaCoordinator, IUploadArticleEntryMediaCoordinator uploadCoordinator, IEntryMediaSearchStrategy searchStrategy)
        {
            _createEntryMediaCoordinator = createEntryMediaCoordinator;
            _uploadCoordinator = uploadCoordinator;
            _searchStrategy = searchStrategy;
        }

        public async Task<EntryMedia> PersistArticleEntryMediaAsync(ArticleEntryMediaRequest entryMediaRequest)
        {
            if (entryMediaRequest == null) throw new ArgumentNullException(nameof(entryMediaRequest));

            var searchTask = _searchStrategy.Find(entryMediaRequest.EntryId, entryMediaRequest.MD5Base64Hash);

            var articleEntryMedia = await _createEntryMediaCoordinator.CreateArticleEntryMedia(entryMediaRequest);

            if (articleEntryMedia != null)
            {
                var existingMediaMatch = await searchTask;
                
                return existingMediaMatch != null ?
                    await StoreBlobUriAndMarkUploadSuccess(articleEntryMedia, StringUtilities.FromBase64(existingMediaMatch.BlobUrl))
                    : await UploadMediaContent(entryMediaRequest, articleEntryMedia);
            }

            return articleEntryMedia;
        }

        private async Task<EntryMedia> UploadMediaContent(ArticleEntryMediaRequest entryMediaRequest, EntryMedia articleEntryMedia)
        {
            var uploadUri = await _uploadCoordinator.UploadEntryMedia(entryMediaRequest, articleEntryMedia);

            if (string.IsNullOrWhiteSpace(uploadUri))
            {
                return articleEntryMedia;
            }

            return await StoreBlobUriAndMarkUploadSuccess(articleEntryMedia, uploadUri);
        }

        private Task<EntryMedia> StoreBlobUriAndMarkUploadSuccess(EntryMedia articleEntryMedia, string uploadUri)
        {
            return _createEntryMediaCoordinator.MarkUploadedSuccess(articleEntryMedia, uploadUri);
        }
    }
}