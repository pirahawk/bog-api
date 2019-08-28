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
                var entryContent = new EntryContentFixture().Build();
                var expectedMediaHashToFind = "hash2";
                var entryMedia1 = new EntryMediaFixture { MD5Base64Hash = "hash1" }.Build();
                var entryMedia2 = new EntryMediaFixture { EntryContentId = entryContent.Id, MD5Base64Hash = expectedMediaHashToFind }.Build();
                var entryMedia3 = new EntryMediaFixture { MD5Base64Hash = expectedMediaHashToFind }.Build();

                yield return new object[]
                {
                    new[] { entryMedia1, entryMedia2, entryMedia3 },
                    entryContent.Id,
                    "someHashThatDoesNotExistst",
                };

                yield return new object[]
                {
                    new[] { entryMedia1, entryMedia3 },
                    entryContent.Id,
                    expectedMediaHashToFind,
                };

                yield return new object[]
                {
                    new[] { entryMedia1, entryMedia2, entryMedia3 },
                    entryContent.Id,
                    expectedMediaHashToFind,
                    entryMedia2
                };
            }
        }

        [Theory]
        [MemberData(nameof(FindTestCases))]
        public async Task FindsMediaEntryIfExists(EntryMedia[] existingMedia, Guid entryContentId, string testMd5HashToFind, EntryMedia expected = null)
        {
            var contextFixture = new MockBlogApiDbContextFixture();
            contextFixture.WithQuery(existingMedia);

            var context = contextFixture.Build();
            var entryMediaSearchStrategy = new EntryMediaSearchStrategyFixture
            {
                Context = context
            }.Build();

            var result = await entryMediaSearchStrategy.Find(entryContentId, testMd5HashToFind);

            contextFixture.Mock.Verify(ctx => ctx.Query<EntryMedia>());

            if (expected == null)
            {
                Assert.Null(result);
                return;
            }

            Assert.Equal(expected, result);
        }
    }
}