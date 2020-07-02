using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Markdown;
using Bog.Api.Domain.Tests.DbContext;
using Moq;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class BogMarkdownConverterStrategyFixture
    {
        public IBogMarkdownConverter BogMarkdownConverter { get; set; }
        public IGetLatestArticleEntryStrategy GetLatestArticleEntryStrategy { get; set; }
        public IBlogApiDbContext Context { get; set; }
        public IUploadArticleEntryCoordinator UploadArticleEntryCoordinator { get; set; }

        public BogMarkdownConverterStrategyFixture()
        {
            BogMarkdownConverter = new Mock<IBogMarkdownConverter>().Object;
            GetLatestArticleEntryStrategy = new GetLatestArticleEntryStrategyFixture().Build();
            Context = new MockBlogApiDbContextFixture().Build();
            UploadArticleEntryCoordinator = new UploadArticleEntryCoordinatorFixture().Build();
        }

        public BogMarkdownConverterStrategy Build()
        {
            return new BogMarkdownConverterStrategy(BogMarkdownConverter, GetLatestArticleEntryStrategy, Context, UploadArticleEntryCoordinator);
        }
    }
}