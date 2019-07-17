using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UpdateArticleCoordinatorTest
    {
        [Fact]
        public async Task ReturnsFalseWhenArticleNotFound()
        {
            var articleIdThatDoesNotExist = Guid.NewGuid();

            var contextFixture = new MockBlogApiDbContextFixture();
            contextFixture.Mock.Setup(ctx => ctx.Find<Article>(It.IsAny<Guid>()));

            var coordinatorFixture = new UpdateArticleCoordinatorFixture
            {
                Context = contextFixture.Build()
            };

            var coordinator = coordinatorFixture.Build();

            var result = await coordinator.TryUpdateArticle(articleIdThatDoesNotExist, new ArticleRequest());

            contextFixture.Mock.Verify(ctx => ctx.Find<Article>(articleIdThatDoesNotExist));
            Assert.False(result);
        }

        [Fact]
        public async Task UpdatesArticleDataWhenFound()
        {
            var existingArticleId = Guid.NewGuid();
            var updatedArticle = new ArticleRequest
            {
                Author = "Second",
                IsPublished = true
            };
            var articleFixture = new ArticleFixture
            {
                Id =  existingArticleId,
                Author = "First"
            };
            var existingArticle = articleFixture.Build();
            var contextFixture = new MockBlogApiDbContextFixture();

            contextFixture.Mock
                .Setup(ctx => ctx.Find<Article>(existingArticleId))
                .Returns(async () =>
                {
                    await Task.CompletedTask;
                    return existingArticle;
                });

            var clock = new MockClock();

            var coordinatorFixture = new UpdateArticleCoordinatorFixture
            {
                Context = contextFixture.Build(),
                Clock = clock
            };

            var coordinator = coordinatorFixture.Build();

            Assert.False(existingArticle.Updated.HasValue);


            Assert.NotEqual(updatedArticle.Author, existingArticle.Author);

            var result = await coordinator.TryUpdateArticle(existingArticleId, updatedArticle);

            contextFixture.Mock.Verify(ctx => ctx.Find<Article>(existingArticleId));
            contextFixture.Mock.Verify(ctx => ctx.SaveChanges());

            Assert.True(result);
            Assert.Equal(updatedArticle.Author, existingArticle.Author);
            Assert.Equal(updatedArticle.IsPublished, existingArticle.IsPublished);
            Assert.Equal(clock.Now, existingArticle.Updated.GetValueOrDefault());
        }
    }
}