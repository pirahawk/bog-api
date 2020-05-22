using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class AddMetaTagForArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public AddMetaTagForArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public AddMetaTagForArticleCoordinator Build()
        {
            return new AddMetaTagForArticleCoordinator(Context);
        }
    }

    public class AddMetaTagForArticleCoordinatorTest
    {
        [Fact]
        public async Task DoesNotAddMetaTagIfArticleNotFound()
        {
            var newTagToAdd = new MetaTagRequest
            {
                ArticleId = Guid.NewGuid(),
                Name = "SomeTag"
            };
            var mockContext = new MockBlogApiDbContextFixture();
            mockContext.Mock.Setup(ctx => ctx.Find<Article>(newTagToAdd.ArticleId)).ReturnsAsync(null as Article);

            var coordinator = new AddMetaTagForArticleCoordinatorFixture
            {
                Context = mockContext.Build()
            }.Build();

            var result = await coordinator.AddArticleMetaTag(newTagToAdd.ArticleId, newTagToAdd);

            Assert.Null(result);
            mockContext.Mock.Verify(ctx => ctx.Add(It.IsAny<MetaTag>()), Times.Never);
            mockContext.Mock.Verify(ctx => ctx.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task SavesNewMetaTagWhenArticleFound()
        {
            var newTagToAdd = new MetaTagRequest
            {
                ArticleId = Guid.NewGuid(),
                Name = "SomeTag"
            };
            var existingArticle = new ArticleFixture().Build();
            var mockContext = new MockBlogApiDbContextFixture();
            mockContext.Mock.Setup(ctx => ctx.Find<Article>(newTagToAdd.ArticleId)).ReturnsAsync(existingArticle);
            var coordinator = new AddMetaTagForArticleCoordinatorFixture
            {
                Context = mockContext.Build()
            }.Build();

            var result = await coordinator.AddArticleMetaTag(newTagToAdd.ArticleId, newTagToAdd);

            mockContext.Mock.Verify(ctx => ctx.Add(result));
            mockContext.Mock.Verify(ctx => ctx.SaveChanges());

            Assert.Equal(result.ArticleId, existingArticle.Id);
            Assert.Equal(result.Article, existingArticle);
            Assert.Equal(result.Name,newTagToAdd.Name);
        }
    }
}