﻿using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategy : ICreateAndPersistArticleEntryMediaStrategy
    {
        private readonly ICreateEntryMediaCoordinator _createEntryMediaCoordinator;
        private readonly IUploadArticleEntryMediaCoordinator _uploadCoordinator;

        public CreateAndPersistArticleEntryMediaStrategy(ICreateEntryMediaCoordinator createEntryMediaCoordinator, IUploadArticleEntryMediaCoordinator uploadCoordinator)
        {
            _createEntryMediaCoordinator = createEntryMediaCoordinator;
            _uploadCoordinator = uploadCoordinator;
        }

        public async Task<EntryMedia> PersistArticleEntryMediaAsync(ArticleEntryMediaRequest entryMediaRequest)
        {
            var articleEntryMedia = await _createEntryMediaCoordinator.CreateArticleEntryMedia(entryMediaRequest);

            if (articleEntryMedia != null)
            {
                var uploadUri = await _uploadCoordinator.UploadEntryMedia(entryMediaRequest, articleEntryMedia);

                if (string.IsNullOrWhiteSpace(uploadUri))
                {
                    return articleEntryMedia;
                }

                return await _createEntryMediaCoordinator.MarkUploadedSuccess(articleEntryMedia, uploadUri);
            }

            return articleEntryMedia;
        }
    }
}