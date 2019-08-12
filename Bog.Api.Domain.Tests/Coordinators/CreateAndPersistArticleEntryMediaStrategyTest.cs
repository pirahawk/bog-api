using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategyTest
    {
        [Fact]
        public async void DoesNotAttemptPersistToBlobStoreIfCannotCreateEntryMedia()
        {
            var mockCoordinator = new Mock<ICreateEntryMediaCoordinator>();
            mockCoordinator.Setup(cc => cc.CreateArticleEntryMedia(It.IsAny<ArticleEntryMediaRequest>())).ReturnsAsync(()=>null);
            mockCoordinator.Setup(cc => cc.MarkUploadedSuccess(It.IsAny<EntryMedia>(), It.IsAny<string>()));

            var mockBlob = new Mock<IUploadArticleEntryMediaCoordinator>();;
            mockBlob.Setup(blob => blob.UploadEntryMedia(It.IsAny<ArticleEntryMediaRequest>(), It.IsAny<EntryMedia>()));

            var strategy = new CreateAndPersistArticleEntryMediaStrategyFixture
            {
                CreateEntryMediaCoordinator = mockCoordinator.Object,
                UploadArticleEntryMediaCoordinator = mockBlob.Object
                
            }.Build();

            var articleEntryMediaRequest = new ArticleEntryMediaRequest();
            var result = await strategy.PersistArticleEntryMediaAsync(articleEntryMediaRequest);

            Assert.Null(result);
            mockBlob.Verify(blob => blob.UploadEntryMedia(articleEntryMediaRequest, It.IsAny<EntryMedia>()), Times.Never);
            mockCoordinator.Verify(cc => cc.MarkUploadedSuccess(It.IsAny<EntryMedia>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async void InvokesPersistToBlobStoreWhenSuccessfullyCreated()
        {
            var entryMedia = new EntryMediaFixture().Build();
            var mockBlobUrl = "someUrl";
            var mockCoordinator = new Mock<ICreateEntryMediaCoordinator>();
            mockCoordinator.Setup(cc => cc.CreateArticleEntryMedia(It.IsAny<ArticleEntryMediaRequest>())).ReturnsAsync(() => entryMedia);
            mockCoordinator.Setup(cc => cc.MarkUploadedSuccess(It.IsAny<EntryMedia>(), It.IsAny<string>())).ReturnsAsync(entryMedia);

            var mockBlob = new Mock<IUploadArticleEntryMediaCoordinator>(); ;
            mockBlob.Setup(blob => blob.UploadEntryMedia(It.IsAny<ArticleEntryMediaRequest>(), It.IsAny<EntryMedia>())).ReturnsAsync(mockBlobUrl);

            var strategy = new CreateAndPersistArticleEntryMediaStrategyFixture
            {
                CreateEntryMediaCoordinator = mockCoordinator.Object,
                UploadArticleEntryMediaCoordinator = mockBlob.Object

            }.Build();

            var articleEntryMediaRequest = new ArticleEntryMediaRequest();
            var result = await strategy.PersistArticleEntryMediaAsync(articleEntryMediaRequest);

            Assert.Equal(entryMedia, result);
            mockBlob.Verify(blob => blob.UploadEntryMedia(articleEntryMediaRequest, entryMedia));
            mockCoordinator.Verify(cc => cc.MarkUploadedSuccess(entryMedia, mockBlobUrl));
        }
    }
}