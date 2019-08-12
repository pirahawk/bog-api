using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateEntryMediaCoordinatorTest
    {
        [Fact]
        public async Task ReturnsNullWhenEntryNotFound()
        {
            var articleEntry = new EntryContentFixture().Build();
            var dbContextFixture = new MockBlogApiDbContextFixture();
            dbContextFixture.Mock.Setup(ctx => ctx.Find<EntryContent>(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var coordinator = new CreateEntryMediaCoordinatorFixture
            {
                Context = dbContextFixture.Build()
            }.Build();

            var result = await coordinator.CreateArticleEntryMedia(new ArticleEntryMediaRequest
            {
                EntryId = articleEntry.Id
            });

            Assert.Null(result);
            dbContextFixture.Mock.Verify(ctx => ctx.Find<EntryContent>(articleEntry.Id));
        }

        [Fact]
        public async Task CreatesNewEntryMediaWhenEntryFound()
        {
            
            var articleEntry = new EntryContentFixture().Build();
            var request = new ArticleEntryMediaRequest
            {
                FileName = "testFile",
                ContentType = "image/png",
                EntryId = articleEntry.Id,
                MD5Base64Hash = "1234",
                MediaContent = Enumerable.Empty<byte>().ToArray()
            };
            var mockClock = new MockClock();
            var dbContextFixture = new MockBlogApiDbContextFixture();
            dbContextFixture.Mock
                .Setup(ctx => ctx.Find<EntryContent>(It.IsAny<Guid>()))
                .ReturnsAsync(() => articleEntry);

            var coordinator = new CreateEntryMediaCoordinatorFixture
            {
                Context = dbContextFixture.Build(),
                Clock = mockClock
            }.Build();

            var result = await coordinator.CreateArticleEntryMedia(request);

            Assert.Equal(articleEntry.Id, result.EntryContentId);
            Assert.Equal(request.FileName, result.FileName);
            Assert.Equal(request.ContentType, result.ContentType);
            Assert.Equal(request.MD5Base64Hash, result.MD5Base64Hash);
            Assert.Equal(mockClock.Now, result.Created);

            dbContextFixture.Mock.Verify(ctx => ctx.Find<EntryContent>(articleEntry.Id));
            dbContextFixture.Mock.Verify(ctx => ctx.Add(result));
            dbContextFixture.Mock.Verify(ctx => ctx.SaveChanges());
        }

        [Fact]
        public async Task MarksEntryMediaAsPersistedWhenSuccessful()
        {
            var expectedUriUpload = "http://test.com";
            var entryMedia = new EntryMediaFixture().Build();
            var mockClock = new MockClock();
            var dbContextFixture = new MockBlogApiDbContextFixture();
            var dbMock = dbContextFixture.Mock;
            dbMock.Setup(ctx => ctx.Attach(entryMedia));

            var coordinator = new CreateEntryMediaCoordinatorFixture
            {
                Context = dbContextFixture.Build(),
                Clock = mockClock
            }.Build();

            var result = await coordinator.MarkUploadedSuccess(entryMedia, expectedUriUpload);

            Assert.Equal( expectedUriUpload, result.BlobUrl);
            Assert.Equal(mockClock.Now, result.Persisted);
            dbMock.Verify(ctx => ctx.Attach(entryMedia));
            dbMock.Verify(ctx => ctx.SaveChanges());
        }
    }
}