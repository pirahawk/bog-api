using Bog.Api.Domain.Coordinators;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class GetArticleMediaLookupStrategyFixture
    {
        public IFindBlogArticleCoordinator FindBlogArticleCoordinator { get; set; }
        public IGetMediaLookupQuery MediaLookupQuery { get; set; }

        public GetArticleMediaLookupStrategyFixture()
        {
            FindBlogArticleCoordinator = new FindBlogArticleCoordinatorFixture().Build();
            MediaLookupQuery = new GetMediaLookupQueryFixture().Build();
        }

        public GetArticleMediaLookupStrategy Build()
        {
            return new GetArticleMediaLookupStrategy(FindBlogArticleCoordinator, MediaLookupQuery);
        }
    }
}