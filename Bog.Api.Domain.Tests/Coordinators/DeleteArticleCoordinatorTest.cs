using System.Threading.Tasks;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class DeleteArticleCoordinatorTest
    {
        [Fact]
        public async Task DoesNothingIfArticleNotFound()
        {
            var article = new ArticleFixture().Build();
            var contextFixture= new MockBlogApiDbContextFixture();
            var mock = contextFixture.Mock;

            var fixture = new DeleteArticleCoordinatorFixture
            {
                Context = contextFixture.Build()
            };

            var coordinator = fixture.Build();

            Assert.False(article.IsDeleted);
            Assert.Null(article.Deleted);

            var deleteResult = await coordinator.TryDeleteArticle(article.Id);

            Assert.False(deleteResult);
            Assert.False(article.IsDeleted);
            Assert.Null(article.Deleted);
            mock.Verify(ctx => ctx.SaveChanges(), Times.Never);
        }

        [Fact]
        public async Task SetsArticleToDeletedWhenFound()
        {
            var article = new ArticleFixture().Build();
            var contextFixture = new MockBlogApiDbContextFixture()
                .With(article, article.Id);

            var mock = contextFixture.Mock;

            var fixture = new DeleteArticleCoordinatorFixture
            {
                Context = contextFixture.Build()
            };

            var coordinator = fixture.Build();

            Assert.False(article.IsDeleted);
            Assert.Null(article.Deleted);

            var deleteResult = await coordinator.TryDeleteArticle(article.Id);

            Assert.True(deleteResult);
            Assert.True(article.IsDeleted);
            Assert.NotNull(article.Deleted);

            mock.Verify(ctx => ctx.SaveChanges());
        }

        [Fact]
        public async Task DoesNothingIfArticleAlreadyMarkedAsDeleted()
        {
            var article = new ArticleFixture
            {
                IsDeleted = true
            }.Build();

            var contextFixture = new MockBlogApiDbContextFixture()
                .With(article, article.Id);

            var mock = contextFixture.Mock;

            var fixture = new DeleteArticleCoordinatorFixture
            {
                Context = contextFixture.Build()
            };

            var coordinator = fixture.Build();

            Assert.True(article.IsDeleted);

            var deleteResult = await coordinator.TryDeleteArticle(article.Id);

            Assert.True(deleteResult);
            mock.Verify(ctx => ctx.SaveChanges(), Times.Never);
        }
    }
}