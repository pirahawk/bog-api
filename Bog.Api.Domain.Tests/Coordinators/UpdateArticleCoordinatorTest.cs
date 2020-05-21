using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using System;
using System.Collections.Generic;
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


        public static IEnumerable<object[]> IncrementalUpdateCases
        {
            get
            {
                var articleFixture = new ArticleFixture
                {
                    Author = "First",
                    Title = "First-Title",
                    Description = "First-Description",
                    IsPublished = false
                };

                var update1 = new ArticleRequest
                {
                    Author = "Second",
                    Title = "Second-Title",
                    Description = "Second-Description",
                    IsPublished = true
                };
                
                Action<Article, ArticleRequest> resultCheck1 = (article, request) =>
                {
                    Assert.Equal(article.Author, request.Author);
                    Assert.Equal(article.Title, request.Title);
                    Assert.Equal(article.Description, request.Description);
                    Assert.Equal(article.IsPublished, request.IsPublished);
                }; 

                yield return new object[] { articleFixture.Build(), update1, resultCheck1 };

                var update2 = new ArticleRequest
                {
                    Author = "Second",
                };

                Action<Article, ArticleRequest> resultCheck2 = (article, request) =>
                {
                    Assert.Equal(article.Author, request.Author);
                    Assert.Equal(article.Title, articleFixture.Title);
                    Assert.Equal(article.Description, articleFixture.Description);
                    Assert.Equal(article.IsPublished, articleFixture.IsPublished);
                };

                yield return new object[] { articleFixture.Build(), update2, resultCheck2 };


                var update3 = new ArticleRequest
                {
                    Title = "Second",
                };

                Action<Article, ArticleRequest> resultCheck3 = (article, request) =>
                {
                    Assert.Equal(article.Author, articleFixture.Author);
                    Assert.Equal(article.Title, request.Title);
                    Assert.Equal(article.Description, articleFixture.Description);
                    Assert.Equal(article.IsPublished, articleFixture.IsPublished);
                };

                yield return new object[] { articleFixture.Build(), update3, resultCheck3 };


                var update4 = new ArticleRequest
                {
                    Description = "Second",
                };

                Action<Article, ArticleRequest> resultCheck4 = (article, request) =>
                {
                    Assert.Equal(article.Author, articleFixture.Author);
                    Assert.Equal(article.Title, articleFixture.Title);
                    Assert.Equal(article.Description, request.Description);
                    Assert.Equal(article.IsPublished, articleFixture.IsPublished);
                };

                yield return new object[] { articleFixture.Build(), update4, resultCheck4 };

                var update5 = new ArticleRequest
                {
                    IsPublished = !articleFixture.IsPublished
                };

                Action<Article, ArticleRequest> resultCheck5 = (article, request) =>
                {
                    Assert.Equal(article.Author, articleFixture.Author);
                    Assert.Equal(article.Title, articleFixture.Title);
                    Assert.Equal(article.Description, articleFixture.Description);
                    Assert.Equal(article.IsPublished, request.IsPublished);
                };

                yield return new object[] { articleFixture.Build(), update5, resultCheck5 };
            }
        }

        [Theory]
        [MemberData(nameof(IncrementalUpdateCases))]
        public async Task IncrementallyUpdatesArticleDataPartsWhenFound(Article existingArticle, ArticleRequest updatedArticle, Action<Article, ArticleRequest> validationAction)
        {
            var contextFixture = new MockBlogApiDbContextFixture();

            contextFixture.Mock
                .Setup(ctx => ctx.Find<Article>(existingArticle.Id))
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

            var result = await coordinator.TryUpdateArticle(existingArticle.Id, updatedArticle);

            contextFixture.Mock.Verify(ctx => ctx.Find<Article>(existingArticle.Id));
            contextFixture.Mock.Verify(ctx => ctx.SaveChanges());

            Assert.True(result);
            Assert.Equal(clock.Now, existingArticle.Updated.GetValueOrDefault());
            validationAction(existingArticle, updatedArticle);
        }
    }
}