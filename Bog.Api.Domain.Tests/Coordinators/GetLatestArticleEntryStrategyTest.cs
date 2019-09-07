using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class GetLatestArticleEntryStrategyTest
    {
        public static IEnumerable<object[]> FindTestCases
        {
            get
            {
                var articleWithNoEntries = new ArticleFixture().Build();

                yield return new object[]
                {
                    new Article[]{ articleWithNoEntries },
                    articleWithNoEntries.Id,
                    null
                };

                var unpublishedEntry = new EntryContentFixture
                {
                    Persisted = null
                }.Build();

                var articleWithNoPublishedEntries = new ArticleFixture()
                    .WithEntry(unpublishedEntry)
                    .Build();

                yield return new object[]
                {
                    new Article[]{ articleWithNoPublishedEntries },
                    articleWithNoPublishedEntries.Id,
                    null
                };

                var mockClock = new MockClock();

                var latestEntry = new EntryContentFixture
                {
                    Persisted = mockClock.Now
                }.Build();

                var olderPublishedEntry = new EntryContentFixture
                {
                    Persisted = mockClock.Now - TimeSpan.FromHours(1)
                }.Build();

                var articleWithPublishedEntries = new ArticleFixture()
                    .WithEntry(unpublishedEntry, olderPublishedEntry, latestEntry)
                    .Build();

                yield return new object[]
                {
                    new Article[]{ articleWithPublishedEntries },
                    articleWithPublishedEntries.Id,
                    latestEntry
                };
            }
        }

        [Theory]
        [MemberData(nameof(FindTestCases))]
        public async Task FindsEntryAsExpected(Article[] articles, Guid articleIdToFind, EntryContent expectedResult=null)
        {
            var mockContext = new MockBlogApiDbContextFixture().WithQuery(articles);
            var dbMock = mockContext.Mock;

            var strategy = new GetLatestArticleEntryStrategyFixture
            {
                Context = mockContext.Build()
            }.Build();

            var result = await strategy.FindLatestEntry(articleIdToFind);

            dbMock.Verify(ctx => ctx.Query<Article>(It.IsAny<string[]>()));

            if (expectedResult == null)
            {
                Assert.Null(result);
                return;
            }

            Assert.Equal(expectedResult, result);
        }
    }
}