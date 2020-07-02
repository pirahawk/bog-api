using System;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Markdown;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Moq;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class BogMarkdownConverterStrategyTest
    {
        [Fact]
        public async Task ReturnsEmptyStringIfLatestArticleEntryNotFound()
        {
            var articleIdToFind = Guid.NewGuid();
            var mockGetLatestArticle = new Mock<IGetLatestArticleEntryStrategy>();
            mockGetLatestArticle.Setup(m => m.FindLatestEntry(articleIdToFind))
                .ReturnsAsync(null as EntryContent).Verifiable();

            var converterStrategy = new BogMarkdownConverterStrategyFixture
            {
                GetLatestArticleEntryStrategy = mockGetLatestArticle.Object
            }.Build();

            var result = await converterStrategy.GetLatestConvertedEntryContentUri(articleIdToFind);

            Assert.True(string.IsNullOrWhiteSpace(result));
            mockGetLatestArticle.VerifyAll();
        }

        [Fact]
        public async Task ReturnsLatestEntryConvertedUrlDecodedIfExists()
        {
            var articleIdToFind = Guid.NewGuid();
            var urlToExpect = "someUrl";

            var latestEntryContent = new EntryContentFixture
                {
                    ArticleId = articleIdToFind,
                    ConvertedBlobUrl = StringUtilities.ToBase64(urlToExpect)
                }
                .Build();


            var mockGetLatestArticle = new Mock<IGetLatestArticleEntryStrategy>();
            mockGetLatestArticle.Setup(m => m.FindLatestEntry(articleIdToFind))
                .ReturnsAsync(latestEntryContent).Verifiable();

            var converterStrategy = new BogMarkdownConverterStrategyFixture
            {
                GetLatestArticleEntryStrategy = mockGetLatestArticle.Object
            }.Build();

            var result = await converterStrategy.GetLatestConvertedEntryContentUri(articleIdToFind);

            Assert.Equal(urlToExpect, result);
            mockGetLatestArticle.VerifyAll();
        }

        [Fact]
        public async Task WillConvertAndSaveMdContentsIfStillOutstanding()
        {
            var articleIdToFind = Guid.NewGuid();
            var mdUrl = "someMdUrl";
            var urlToExpect = "someUrl";
            var mockConvertedContent = "someContent";

            var latestEntryContent = new EntryContentFixture
                {
                    ArticleId = articleIdToFind,
                    BlobUrl = StringUtilities.ToBase64(mdUrl),
                    ConvertedBlobUrl = string.Empty
                }
                .Build();

            var mockGetLatestArticle = new Mock<IGetLatestArticleEntryStrategy>();
            mockGetLatestArticle.Setup(m => m.FindLatestEntry(articleIdToFind))
                .ReturnsAsync(latestEntryContent).Verifiable();

            var mockMarkdownConverter = new Mock<IBogMarkdownConverter>();
            mockMarkdownConverter.Setup(m => m.ConvertArticle(articleIdToFind, mdUrl))
                .ReturnsAsync(mockConvertedContent).Verifiable();

            var mockUploadArticleEntryCoordinator = new Mock<IUploadArticleEntryCoordinator>();
            mockUploadArticleEntryCoordinator
                .Setup(m => m.UploadConvertedArticleEntry(latestEntryContent, mockConvertedContent))
                .ReturnsAsync(urlToExpect).Verifiable();

            var mockBlogApiDbContextFixture = new MockBlogApiDbContextFixture();

            var converterStrategy = new BogMarkdownConverterStrategyFixture
            {
                GetLatestArticleEntryStrategy = mockGetLatestArticle.Object,
                BogMarkdownConverter = mockMarkdownConverter.Object,
                UploadArticleEntryCoordinator = mockUploadArticleEntryCoordinator.Object,
                Context = mockBlogApiDbContextFixture.Build()

            }.Build();

            var result = await converterStrategy.GetLatestConvertedEntryContentUri(articleIdToFind);

            Assert.Equal(urlToExpect, result);

            mockGetLatestArticle.VerifyAll();
            mockMarkdownConverter.VerifyAll();
            mockUploadArticleEntryCoordinator.VerifyAll();
            mockBlogApiDbContextFixture.Mock.Verify(m => m.Attach(latestEntryContent), Times.Once);
            mockBlogApiDbContextFixture.Mock.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}