using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using System;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Models.Http;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateArticleEntryCoordinatorTest
    {
        [Fact]
        public async Task ReturnsNullWhenArticleNotFound()
        {
            var article = new ArticleFixture().Build();
            var dbContextFixture = new MockBlogApiDbContextFixture();
            dbContextFixture.Mock
                .Setup(ctx => ctx.Find<Article>(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var createArticleEntryCoordinator = new CreateArticleEntryCoordinatorFixture
            {
                Context = dbContextFixture.Build()
            }.Build();

            var result = await createArticleEntryCoordinator.CreateArticleEntry(article.Id, new ArticleEntry());


            Assert.Null(result);
            dbContextFixture.Mock.Verify(ctx => ctx.Find<Article>(article.Id));
        }

        [Fact]
        public async Task CreatesNewEntryWhenArticleFound()
        {
            var article = new ArticleFixture().Build();
            var mockClock = new MockClock();
            var dbContextFixture = new MockBlogApiDbContextFixture();
            dbContextFixture.Mock
                .Setup(ctx => ctx.Find<Article>(It.IsAny<Guid>()))
                .ReturnsAsync(() => article);

            var createArticleEntryCoordinator = new CreateArticleEntryCoordinatorFixture
            {
                Context = dbContextFixture.Build(),
                Clock = mockClock
            }.Build();

            var result = await createArticleEntryCoordinator.CreateArticleEntry(article.Id, new ArticleEntry());

            Assert.Equal( article.Id, result.ArticleId);
            Assert.Equal(mockClock.Now, result.Created);

            dbContextFixture.Mock.Verify(ctx => ctx.Find<Article>(article.Id));
            dbContextFixture.Mock.Verify(ctx => ctx.Add(result));
            dbContextFixture.Mock.Verify(ctx => ctx.SaveChanges());
        }

        [Fact]
        public async Task UpdatesAndPersistsEntryContentOnUpload()
        {
            var entryContent = new EntryContentFixture().Build();
            var blobUrl = "http://somewhere";
            var blobUrlBase64 = StringUtilities.ToBase64(blobUrl);
            var mockClock = new MockClock();
            var dbContextFixture = new MockBlogApiDbContextFixture();
            var createArticleEntryCoordinator = new CreateArticleEntryCoordinatorFixture
            {
                Context = dbContextFixture.Build(),
                Clock = mockClock
            }.Build();

            var resultEntryContent = await createArticleEntryCoordinator.MarkUploadSuccess(entryContent, blobUrl);

            Assert.Equal(blobUrlBase64, resultEntryContent.BlobUrl);

            dbContextFixture.Mock.Verify(ctx => ctx.Attach(entryContent));
            dbContextFixture.Mock.Verify(ctx => ctx.SaveChanges());

        }
    }
}