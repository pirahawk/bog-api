using System;
using Bog.Api.Domain.Coordinators;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateAndPersistArticleEntryStrategyTest
    {
        [Fact]
        public async Task DoesNotAttemptToPeristToBlobStoreIfCannotCreateArticleEntry()
        {
            var createMock = new Mock<ICreateArticleEntryCoordinator>();
            var uploadMock = new Mock<IUploadArticleEntryCoordinator>();

            createMock.Setup(c => c.CreateArticleEntry(It.IsAny<Guid>(), It.IsAny<ArticleEntry>()))
                .ReturnsAsync(() => null);

            uploadMock.Setup(u => u.UploadArticleEntry(It.IsAny<EntryContent>(), It.IsAny<ArticleEntry>())).Verifiable();

            var persistArticleEntryStrategy = new CreateAndPersistArticleEntryStrategyFixture
            {
                CreateEntryCoordinator = createMock.Object,
                UploadCoordinator = uploadMock.Object
            }.Build();

            var result = await persistArticleEntryStrategy.PersistArticleEntryAsync(Guid.NewGuid(), new ArticleEntry());

            uploadMock.Verify(u => u.UploadArticleEntry(It.IsAny<EntryContent>(), It.IsAny<ArticleEntry>()), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public async Task InvokesPersistToBlobStoreWhenSuccessfullyCreated()
        {
            var entryContent = new EntryContentFixture().Build();
            var entry = new ArticleEntry();
            var createMock = new Mock<ICreateArticleEntryCoordinator>();
            var uploadMock = new Mock<IUploadArticleEntryCoordinator>();

            createMock.Setup(c => c.CreateArticleEntry(It.IsAny<Guid>(), It.IsAny<ArticleEntry>()))
                .ReturnsAsync(() => entryContent);

            uploadMock.Setup(u => u.UploadArticleEntry(entryContent, It.IsAny<ArticleEntry>())).Verifiable();

            var persistArticleEntryStrategy = new CreateAndPersistArticleEntryStrategyFixture
            {
                CreateEntryCoordinator = createMock.Object,
                UploadCoordinator = uploadMock.Object
            }.Build();

            var result = await persistArticleEntryStrategy.PersistArticleEntryAsync(Guid.NewGuid(), new ArticleEntry());

            uploadMock.VerifyAll();
            Assert.Equal(entryContent, result);
        }
    }
}