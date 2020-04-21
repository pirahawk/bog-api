using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class GetArticleMediaLookupStrategyTest
    {
        [Fact]
        public async Task DoesNotCallDownToQueryIfArticleDoesNotExist()
        {
            var articleIdToLookup = Guid.NewGuid();
            var mockContextWithNoArticles = new MockBlogApiDbContextFixture();
            mockContextWithNoArticles.Mock.Setup(m => m.Find<Article>(articleIdToLookup)).ReturnsAsync(null as Article);

            var mockQuery = new Mock<IGetMediaLookupQuery>();
            mockQuery.Setup(m => m.CreateMediaLookup(articleIdToLookup));

            var blogArticleCoordinatorFixture = new FindBlogArticleCoordinatorFixture
            {
                Context = mockContextWithNoArticles.Build(),
            };

            var lookupStrategy = new GetArticleMediaLookupStrategyFixture
            {
                FindBlogArticleCoordinator = blogArticleCoordinatorFixture.Build(),
                MediaLookupQuery = mockQuery.Object
            }.Build();

            var result = await lookupStrategy.GetMediaLookup(articleIdToLookup);

            Assert.Null(result);
            mockQuery.Verify(m => m.CreateMediaLookup(articleIdToLookup), Times.Never);
        }

        [Fact]
        public async Task CallsDownToQueryIfArticleDoesExist()
        {
            var articleIdToLookup = Guid.NewGuid();
            var articleToBuildFor = new ArticleFixture
            {
                Id = articleIdToLookup
            }.Build();

            var mockMapping = new Dictionary<string,string>();
            var mockContextWithNoArticles = new MockBlogApiDbContextFixture();
            mockContextWithNoArticles.Mock.Setup(m => m.Find<Article>(articleIdToLookup)).ReturnsAsync(articleToBuildFor);

            var mockQuery = new Mock<IGetMediaLookupQuery>();
            mockQuery.Setup(m => m.CreateMediaLookup(articleIdToLookup)).Returns(mockMapping);

            var blogArticleCoordinatorFixture = new FindBlogArticleCoordinatorFixture
            {
                Context = mockContextWithNoArticles.Build(),
            };

            var lookupStrategy = new GetArticleMediaLookupStrategyFixture
            {
                FindBlogArticleCoordinator = blogArticleCoordinatorFixture.Build(),
                MediaLookupQuery = mockQuery.Object
            }.Build();

            var result = await lookupStrategy.GetMediaLookup(articleIdToLookup);

            Assert.Equal(mockMapping, result.MediaLookup);

            mockQuery.Verify(m => m.CreateMediaLookup(articleIdToLookup), Times.Once);
        }
    }
}