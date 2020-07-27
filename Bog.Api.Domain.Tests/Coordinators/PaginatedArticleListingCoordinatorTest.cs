using System;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Models.Http;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class PaginatedArticleListingCoordinatorTest
    {
        public static IEnumerable<object[]> PaginationTestCases
        {
            get
            {
                var idRange = Enumerable.Range(1, 100);
                var clock = new MockClock();

                var testArticles = idRange
                    .Select(index =>
                    {
                        return new ArticleFixture()
                        {
                            Title = $"{index}",
                            Created = clock.MockTime.Value + TimeSpan.FromSeconds(index)
                        }.Build();
                    });

                var first10ExpectedArticlesRange = idRange
                    .OrderByDescending(id => id)
                    .Take(10);
                yield return new object[] { testArticles, first10ExpectedArticlesRange, 0, 10 };

                var assertFirst10ArticlesSkippedReturned = idRange
                    .OrderByDescending(id => id)
                    .Skip(10)
                    .Take(10);
                yield return new object[] { testArticles, assertFirst10ArticlesSkippedReturned, 10, 10 };

                var assertAllArticlesReturned = idRange
                    .OrderByDescending(id => id);

                yield return new object[] { testArticles, assertAllArticlesReturned };
            }
        }

        public static IEnumerable<object[]> FilterTestCases
        {
            get
            {
                var metaTag1 = new MetaTagRequest { Name = "tag1"};
                var metaTag2 = new MetaTagRequest { Name = "tag2" };
                var metaTag3 = new MetaTagRequest { Name = "tag3" };
                var metaTag4 = new MetaTagRequest { Name = "tag4" };

                var article1 = new ArticleFixture().WithTags(new []{metaTag1, metaTag2, metaTag3, metaTag4 }).Build();
                var article2 = new ArticleFixture().WithTags(new[] { metaTag1}).Build();
                var article3 = new ArticleFixture().WithTags(new[] { metaTag2, metaTag4 }).Build();
                var article4 = new ArticleFixture().Build();

                var articles = new[]{ article1, article2, article3, article4 };

                yield return new object[] { articles, articles, null, null};
                yield return new object[] { articles, new[] { article1, article2 }, new string[]{ metaTag1.Name }, null};
                yield return new object[] { articles, new[] { article3, article4 }, null, new string[] { metaTag1.Name } };
                yield return new object[] { articles, new[] { article3 }, new string[] { metaTag4.Name } , new string[] { metaTag1.Name } };
                yield return new object[] { articles, Enumerable.Empty<Article>(), new string[] { metaTag1.Name }, new string[] { metaTag1.Name } };
            }
        }

        [Theory]
        [MemberData(nameof(FilterTestCases))]
        public async Task CanFilterAsExpected(IEnumerable<Article> articles, IEnumerable<Article> expectedResults, string[] include = null, string[] filter = null)
        {
            var coordinator = SetupCoordinator(articles);
            var results = await coordinator.Find(Guid.Empty, null, null, filter, include);
            Assert.True(expectedResults.All(expected => results.Contains(expected)));
            Assert.Equal(results.Count(), expectedResults.Count());
        }

        [Theory]
        [MemberData(nameof(PaginationTestCases))]
        public async Task CanPaginateAsExpected(IEnumerable<Article> articles, IEnumerable<int> expectedIdRange, int? skip = null, int? take = null)
        {
            var coordinator = SetupCoordinator(articles);
            var results = await coordinator.Find(Guid.Empty, skip, take, null, null);

            Assert.True(results.All(article => expectedIdRange.Contains(int.Parse(article.Title))));
            Assert.Equal(results.Count(), expectedIdRange.Count());
        }

        private static PaginatedArticleListingCoordinator SetupCoordinator(IEnumerable<Article> articles)
        {
            var mockListingCoordinator = new Mock<IArticleSearchListingCoordinator>();
            mockListingCoordinator.Setup(m => m.Find(It.IsAny<Guid>()))
                .ReturnsAsync(articles.AsQueryable())
                .Verifiable();

            var coordinator = new PaginatedArticleListingCoordinatorFixture
            {
                ArticleSearchListingCoordinator = mockListingCoordinator.Object
            }.Build();
            return coordinator;
        }
    }
}