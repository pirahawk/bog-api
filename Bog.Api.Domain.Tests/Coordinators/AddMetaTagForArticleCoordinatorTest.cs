using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class AddMetaTagForArticleCoordinatorTest
    {
        [Fact]
        public async Task DoesNotAddMetaTagIfArticleDoesNotExist()
        {
            var newTagToAdd = new MetaTagRequest
            {
                Name = "SomeTag"
            };
            var mockContext = new MockBlogApiDbContextFixture();
            mockContext.Mock.Setup(ctx => ctx.Query<Article>(It.IsAny<string>()))
                .Returns(Enumerable.Empty<Article>().AsQueryable());

            var coordinator = new AddMetaTagForArticleCoordinatorFixture
            {
                Context = mockContext.Build()
            }.Build();

            var result = await coordinator.AddArticleMetaTags(Guid.NewGuid(), newTagToAdd);

            Assert.Null(result);
            mockContext.Mock.Verify(ctx => ctx.Add(It.IsAny<MetaTag[]>()), Times.Never);
            mockContext.Mock.Verify(ctx => ctx.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task DoesNotAddNewTagsIfTheyAlreadyExistOnArticle()
        {
            var newTagToAdd = new MetaTagRequest
            {
                Name = "SomeTag"
            };
            var existingArticleWithTags = new ArticleFixture().WithTags(newTagToAdd).Build();

            var mockContext = new MockBlogApiDbContextFixture();
            mockContext.Mock.Setup(ctx => ctx.Query<Article>(It.IsAny<string>()))
                .Returns(new[] { existingArticleWithTags }.AsQueryable());


            var coordinator = new AddMetaTagForArticleCoordinatorFixture
            {
                Context = mockContext.Build()
            }.Build();

            var result = await coordinator.AddArticleMetaTags(existingArticleWithTags.Id, newTagToAdd);

            Assert.Null(result);
            mockContext.Mock.Verify(ctx => ctx.Add(It.IsAny<MetaTag[]>()), Times.Never);
            mockContext.Mock.Verify(ctx => ctx.SaveChanges(), Times.Never);
        }


        [Fact]
        public async Task PersistsTagsThatDoNotAlreadyExistOnArticle()
        {
            var tagThatExists = new MetaTagRequest
            {
                Name = "tag1"
            };

            var tagThatDoesNotExist = new MetaTagRequest
            {
                Name = "tag2"
            };

            var existingArticleWithTags = new ArticleFixture().WithTags(tagThatExists).Build();
            var mockContext = new MockBlogApiDbContextFixture();
            mockContext.Mock.Setup(ctx => ctx.Query<Article>(It.IsAny<string>()))
                .Returns(new []{existingArticleWithTags}.AsQueryable());

            var coordinator = new AddMetaTagForArticleCoordinatorFixture
            {
                Context = mockContext.Build()
            }.Build();

            var result = await coordinator.AddArticleMetaTags(existingArticleWithTags.Id, tagThatExists, tagThatDoesNotExist);

            Assert.True(result.Any(mt=> mt.Name == tagThatDoesNotExist.Name));
            Assert.True(result.All(mt => mt.Name != tagThatExists.Name));

            mockContext.Mock.Verify(ctx => ctx.Add(result), Times.Once);
            mockContext.Mock.Verify(ctx => ctx.SaveChanges(), Times.Once);
        }
    }
}