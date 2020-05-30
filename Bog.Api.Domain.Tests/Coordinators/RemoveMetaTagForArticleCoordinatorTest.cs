using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class RemoveMetaTagForArticleCoordinatorTest
    {
        [Fact]
        public async Task DoesNotDeleteTagsIfArticleDoesNotExist()
        {
            var metaTag = new MetaTagRequest
            {
                Name = "SomeTag"
            };
            var mockContext = new MockBlogApiDbContextFixture();
            mockContext.Mock.Setup(ctx => ctx.Query<Article>(It.IsAny<string>()))
                .Returns(Enumerable.Empty<Article>().AsQueryable());

            var coordinator = new RemoveMetaTagForArticleCoordinatorFixture()
            {
                Context = mockContext.Build()
            }.Build();

            await coordinator.RemoveArticleMetaTags(Guid.NewGuid(), metaTag);

            mockContext.Mock.Verify(ctx => ctx.Delete(It.IsAny<MetaTag>()), Times.Never);
            mockContext.Mock.Verify(ctx => ctx.SaveChanges(), Times.Never);
        }
    }
}