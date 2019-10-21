using System.Threading.Tasks;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.BlobStore;
using Bog.Api.Domain.Tests.Data;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UploadArticleEntryMediaCoordinatorTest
    {
        [Fact]
        public async Task CallsDownToBlobStoreToPersistMedia()
        {
            var entryMedia = new EntryMediaFixture().Build();
            var entryMediaRequest = new ArticleEntryMediaRequest
            {
                MediaContent = new byte[1]
            };
            var blobStoreFixture = new BlobStoreFixture();
            var mock = blobStoreFixture.Mock;
            var coordinator = new UploadArticleEntryMediaCoordinatorFixture
            {
                BlobStore = blobStoreFixture.Build()
            }.Build();

            var uploadEntryMedia = coordinator.UploadEntryMedia(entryMediaRequest, entryMedia);

            mock.Verify(bs => bs.PersistArticleEntryMedia(entryMedia.Id, 
                entryMedia.EntryContentId, 
                entryMediaRequest.MediaContent, 
                entryMediaRequest.ContentType));

            await Task.CompletedTask;
        }
    }
}