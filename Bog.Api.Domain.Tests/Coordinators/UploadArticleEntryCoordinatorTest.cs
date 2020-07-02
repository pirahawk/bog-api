using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.BlobStore;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Values;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UploadArticleEntryCoordinatorTest
    {
        [Fact]
        public async Task CallsDownToPersistEntryContentToBlobStore()
        {
            var articleEntry = new ArticleEntry
            {
                Content = "Blog Content"
            };

            var entryContent = BuildTestSetup(articleEntry, out var base64, out var mock, out var coordinator);

            var result = await coordinator.UploadMarkdownArticleEntry(entryContent, articleEntry);

            mock.Verify(m => m.PersistArticleEntryAsync(BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER, 
                entryContent.ArticleId, 
                entryContent.Id, 
                base64));

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DoesNotStoreContentIfContentEmpty()
        {
            var articleEntry = new ArticleEntry
            {
                Content = string.Empty
            };

            var entryContent = BuildTestSetup(articleEntry, out var base64, out var mock, out var coordinator);

            var result = await coordinator.UploadMarkdownArticleEntry(entryContent, articleEntry);

            Assert.Null(result);
            mock.Verify(m => m.PersistArticleEntryAsync(BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER,
                entryContent.ArticleId,
                entryContent.Id,
                base64), Times.Never);
        }

        private static EntryContent BuildTestSetup(ArticleEntry articleEntry, out string base64, out Mock<IBlobStore> mock,
            out UploadArticleEntryCoordinator coordinator)
        {
            base64 = string.IsNullOrWhiteSpace(articleEntry.Content)? "foo" : StringUtilities.ToBase64(articleEntry.Content);
            var entryContent = new EntryContentFixture().Build();
            var blobStoreFixture = new BlobStoreFixture();
            mock = blobStoreFixture.Mock;
            var uploadFixture = new UploadArticleEntryCoordinatorFixture
            {
                BlobStore = blobStoreFixture.Build()
            };
            coordinator = uploadFixture.Build();
            return entryContent;
        }
    }
}
