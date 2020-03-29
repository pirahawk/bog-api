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
        public static IEnumerable<object[]> FindHashOnlyTestCases
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

        public static IEnumerable<object[]> FindHashAndFileNameTestCases
        {
            get
            {
                var expectedMediaHashToFind = "hash2";
                var expectedFileNameToFind = "testFile.txt";

                var entryMedia1 = new EntryMediaFixture { MD5Base64Hash = "hash1", FileName = "someFile1.txt"}.Build();
                var entryMedia2 = new EntryMediaFixture { MD5Base64Hash = expectedMediaHashToFind, FileName = expectedFileNameToFind }.Build();
                var entryMedia3 = new EntryMediaFixture { MD5Base64Hash = expectedMediaHashToFind, FileName = "someFile2.txt" }.Build();

                var entryContent1 = new EntryContentFixture().WithMedia(entryMedia1, entryMedia2).Build();
                var entryContent2 = new EntryContentFixture().WithMedia(entryMedia3).Build();

                var article1 = new ArticleFixture().WithEntry(entryContent1).Build();
                var article2 = new ArticleFixture().WithEntry(entryContent2).Build();


                yield return new object[]
                {
                    new[] { article1, article2},
                    entryContent1.Id,
                    "someHashThatDoesNotExistst",
                    expectedFileNameToFind
                };

                yield return new object[]
                {
                    new[] { article1, article2},
                    entryContent1.Id,
                    expectedMediaHashToFind,
                    "fileNameThatDoesNotExist"
                };

                yield return new object[]
                {
                    new[] { article2},
                    entryContent1.Id,
                    expectedMediaHashToFind,
                    expectedFileNameToFind
                };

                yield return new object[]
                {
                    new[] { article1, article2},
                    entryContent1.Id,
                    expectedMediaHashToFind,
                    expectedFileNameToFind,
                    entryMedia2
                };
            }
        }

        [Theory]
        [MemberData(nameof(FindHashOnlyTestCases))]
        public async Task FindsMediaEntryByHashIfExists(Article[] allArticles, Guid entryContentId, string testMd5HashToFind, EntryMedia expected = null)
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

        [Theory]
        [MemberData(nameof(FindHashAndFileNameTestCases))]
        public async Task FindsMediaEntryByHashAndFileNameIfExists(Article[] allArticles, Guid entryContentId, string testMd5HashToFind, string fileNameToFind, EntryMedia expected = null)
        {
            var contextFixture = new MockBlogApiDbContextFixture();
            contextFixture.WithQuery(allArticles);

            var context = contextFixture.Build();
            var entryMediaSearchStrategy = new EntryMediaSearchStrategyFixture
            {
                Context = context
            }.Build();

            var result = await entryMediaSearchStrategy.Find(entryContentId, testMd5HashToFind, fileNameToFind);

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