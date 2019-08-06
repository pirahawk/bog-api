using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.BlobStore;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Values;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UploadArticleEntryCoordinatorTest
    {
        [Fact]
        public async Task CallsDownToPersistEntryContentToBlobStore()
        {
            var entryContent = new EntryContentFixture().Build();
            var articleEntry = new ArticleEntry
            {
                Content = "Blog Content"
            };
            var base64 = StringUtilities.ToBase64(articleEntry.Content);
            var blobStoreFixture = new BlobStoreFixture();
            var mock = blobStoreFixture.Mock;
            var uploadFixture = new UploadArticleEntryCoordinatorFixture
            {
                BlobStore = blobStoreFixture.Build()
            };
            var coordinator = uploadFixture.Build();

            await coordinator.UploadArticleEntry(entryContent, articleEntry);

            mock.Verify(m => m.PersistArticleEntryAsync(BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER, 
                entryContent.ArticleId, 
                entryContent.Id, 
                base64));
        }
    }
}
