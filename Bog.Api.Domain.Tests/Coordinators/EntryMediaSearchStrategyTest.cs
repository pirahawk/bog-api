using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class EntryMediaSearchStrategyTest
    {
        public static IEnumerable<object[]> FindTestCases
        {
            get
            {
                var expectedMediaHashToFind = "hash2";
                var entryMedia1 = new EntryMediaFixture { MD5Base64Hash = "hash1" }.Build();
                var entryMedia2 = new EntryMediaFixture { MD5Base64Hash = expectedMediaHashToFind }.Build();
                var entryMedia3 = new EntryMediaFixture { MD5Base64Hash = expectedMediaHashToFind }.Build();

                var entryContent1 = new EntryContentFixture().WithMedia(entryMedia1, entryMedia2).Build();
                var entryContent2 = new EntryContentFixture().WithMedia(entryMedia3).Build();

                var article1 = new ArticleFixture().WithEntry(entryContent1).Build();
                var article2 = new ArticleFixture().WithEntry(entryContent2).Build();


                yield return new object[]
                {
                    new[] { article1, article2},
                    entryContent1.Id,
                    "someHashThatDoesNotExistst",
                };

                yield return new object[]
                {
                    new[] { article2},
                    entryContent1.Id,
                    expectedMediaHashToFind,
                };

                yield return new object[]
                {
                    new[] { article1, article2},
                    entryContent1.Id,
                    expectedMediaHashToFind,
                    entryMedia2
                };
            }
        }

        [Theory]
        [MemberData(nameof(FindTestCases))]
        public async Task FindsMediaEntryIfExists(Article[] allArticles, Guid entryContentId, string testMd5HashToFind, EntryMedia expected = null)
        {
            var contextFixture = new MockBlogApiDbContextFixture();
            contextFixture.WithQuery(allArticles);

            var context = contextFixture.Build();
            var entryMediaSearchStrategy = new EntryMediaSearchStrategyFixture
            {
                Context = context
            }.Build();

            var result = await entryMediaSearchStrategy.Find(entryContentId, testMd5HashToFind);

            contextFixture.Mock.Verify(ctx => ctx.Query<Article>());

            if (expected == null)
            {
                Assert.Null(result);
                return;
            }

            Assert.Equal(expected, result);
        }
    }
}