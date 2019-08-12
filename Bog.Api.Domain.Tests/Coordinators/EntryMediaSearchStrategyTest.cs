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
                var entryMedia1 = new EntryMediaFixture { MD5Base64Hash = "hash1" }.Build();
                var entryMedia2 = new EntryMediaFixture { MD5Base64Hash = "hash2" }.Build();
                var allMedia = new[] { entryMedia1, entryMedia2 };

                yield return new object[]
                {
                    allMedia,
                    "someHashThatDoesNotExistst",
                };

                yield return new object[]
                {
                    allMedia,
                    entryMedia2.MD5Base64Hash,
                    entryMedia2
                };
            }
        }

        [Theory]
        [MemberData(nameof(FindTestCases))]
        public async Task FindsMediaEntryIfExists(EntryMedia[] existingMedia, string testMD5HashToFind, EntryMedia expected = null)
        {
            var contextFixture = new MockBlogApiDbContextFixture();
            contextFixture.WithQuery(existingMedia);

            var context = contextFixture.Build();
            var entryMediaSearchStrategy = new EntryMediaSearchStrategyFixture
            {
                Context = context
            }.Build();

            var result = await entryMediaSearchStrategy.Find(testMD5HashToFind);

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