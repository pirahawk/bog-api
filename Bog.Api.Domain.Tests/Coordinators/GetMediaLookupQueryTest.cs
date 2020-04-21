using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class GetMediaLookupQueryTest
    {
        public static IEnumerable<object[]> MediaLookupTestCases
        {
            get
            {
                var articleWithNoEntriesFixture = new ArticleFixture();
                yield return new object[]
                {
                    Guid.NewGuid(),
                    articleWithNoEntriesFixture.Build()
                };

                var entryContentWithNoMediaFixture = new EntryContentFixture();
                var articleWithEntriesAndNoImagesFixture = new ArticleFixture().WithEntry(entryContentWithNoMediaFixture.Build());
                var articleWithEntriesAndNoImages = articleWithEntriesAndNoImagesFixture.Build();

                yield return new object[]
                {
                    articleWithEntriesAndNoImages.Id,
                    articleWithEntriesAndNoImages
                };

                var notPersistedEntryMedia = new EntryMediaFixture
                {
                    FileName = "f1.txt",
                    Persisted = null
                }.Build();

                var persistedEntryMedia = new EntryMediaFixture
                {
                    FileName = "f2.txt",
                }.Build();

                var entryContentWithSomeNonPersistedMedia = new EntryContentFixture()
                    .WithMedia(notPersistedEntryMedia, persistedEntryMedia).Build();

                var articleWithSomeNonPersistedMediaFixture = new ArticleFixture()
                    .WithEntry(entryContentWithSomeNonPersistedMedia);
                
                var articleWithSomeNonPersistedMedia = articleWithSomeNonPersistedMediaFixture.Build();
                
                yield return new object[]
                {
                    articleWithSomeNonPersistedMedia.Id,
                    articleWithSomeNonPersistedMedia,
                    new Dictionary<string,string>()
                    {
                        {persistedEntryMedia.FileName, persistedEntryMedia.BlobUrl}
                    }, 
                    new []{ notPersistedEntryMedia.FileName }
                };

                var mockClock = new MockClock();
                var persistTimeStampInThePast = mockClock.Now - TimeSpan.FromMilliseconds(1);
                var persistTimeStampInTheFuture = mockClock.Now + TimeSpan.FromMilliseconds(1);
                var mediaFileCommonName = "common-file.txt";

                var mediaWithSameNameUploadedInThePast = new EntryMediaFixture
                {
                    FileName = mediaFileCommonName,
                    Persisted = persistTimeStampInThePast,
                    BlobUrl = "past"
                }.Build();

                var mediaUploadedInThePast = new EntryMediaFixture
                {
                    FileName = "File1.txt",
                    Persisted = persistTimeStampInThePast,
                    BlobUrl = "past"
                }.Build();

                var oldEntryContentWithMediaFileUploadedInPast = new EntryContentFixture()
                    .WithMedia(mediaWithSameNameUploadedInThePast, mediaUploadedInThePast).Build();

                var mediaWithSameNameUploadedInFuture = new EntryMediaFixture
                {
                    FileName = mediaFileCommonName,
                    Persisted = persistTimeStampInTheFuture,
                    BlobUrl = "future"
                }.Build();

                var mediaUploadedInTheFuture = new EntryMediaFixture
                {
                    FileName = "File2.txt",
                    Persisted = persistTimeStampInTheFuture,
                    BlobUrl = "future"
                }.Build();

                var latestEntryContentWithMediaFileUploadedInFuture = new EntryContentFixture()
                    .WithMedia(mediaWithSameNameUploadedInFuture, mediaUploadedInTheFuture).Build();

                var articleWithMultipleEntriesFixture = new ArticleFixture()
                    .WithEntry(oldEntryContentWithMediaFileUploadedInPast, latestEntryContentWithMediaFileUploadedInFuture);

                var articleWithMultipleEntries = articleWithMultipleEntriesFixture.Build();

                yield return new object[]
                {
                    articleWithMultipleEntries.Id,
                    articleWithMultipleEntries,
                    new Dictionary<string,string>()
                    {
                        {mediaFileCommonName, mediaWithSameNameUploadedInFuture.BlobUrl},
                        {mediaUploadedInThePast.FileName, mediaUploadedInThePast.BlobUrl},
                        {mediaUploadedInTheFuture.FileName, mediaUploadedInTheFuture.BlobUrl},
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(MediaLookupTestCases))]
        public void CompilesDictionaryAsExpected(Guid articleId, 
            Article articleToFind, 
            Dictionary<string,string> expectedResult = null, 
            string[] shouldNotContainFiles = null)
        {
            var articlesQuery = articleToFind == null ? Enumerable.Empty<Article>() : new[] { articleToFind };
            var mockBlogApiDbContextFixture = new MockBlogApiDbContextFixture().WithQuery(articlesQuery);
            var mockDb = mockBlogApiDbContextFixture.Mock;
            var mediaLookupQuery = new GetMediaLookupQueryFixture
            {
                Context = mockBlogApiDbContextFixture.Build()
            }.Build();

            var result = mediaLookupQuery.CreateMediaLookup(articleId);

            if (expectedResult == null)
            {
                Assert.Empty(result.Keys);
                return;
            }

            Assert.Equal(expectedResult.Keys.Count, result.Keys.Count);

            foreach (var imageFileName in expectedResult.Keys)
            {
                Assert.True(result.ContainsKey(imageFileName));
                Assert.Equal(expectedResult[imageFileName], result[imageFileName]);
            }

            if (shouldNotContainFiles != null && shouldNotContainFiles.Any())
            {
                foreach (var keyWhichShouldNotExist in shouldNotContainFiles)
                {
                    Assert.False(result.ContainsKey(keyWhichShouldNotExist));
                }
            }
        }
    }
}