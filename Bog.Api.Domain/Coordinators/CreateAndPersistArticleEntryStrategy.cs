using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateAndPersistArticleEntryStrategy : ICreateAndPersistArticleEntryStrategy
    {
        private readonly ICreateArticleEntryCoordinator _createEntryCoordinator;
        private readonly IUploadArticleEntryCoordinator _uploadCoordinator;

        public CreateAndPersistArticleEntryStrategy(ICreateArticleEntryCoordinator createEntryCoordinator, IUploadArticleEntryCoordinator uploadCoordinator)
        {
            _createEntryCoordinator = createEntryCoordinator;
            _uploadCoordinator = uploadCoordinator;
        }

        public async Task<EntryContent> PersistArticleEntryAsync(Guid articleId, ArticleEntry entry)
        {
            var result = await _createEntryCoordinator.CreateArticleEntry(articleId, entry);

            if (result != null)
            {
                string uploadUrl = await _uploadCoordinator.UploadArticleEntry(result, entry);

                if (string.IsNullOrWhiteSpace(uploadUrl))
                {
                    return result;
                }

                return await _createEntryCoordinator.MarkUploadSuccess(result, uploadUrl);
            }

            return result;
        }
    }
}