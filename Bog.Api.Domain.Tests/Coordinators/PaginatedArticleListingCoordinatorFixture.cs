using Bog.Api.Domain.Coordinators;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class PaginatedArticleListingCoordinatorFixture
    {
        public IArticleSearchListingCoordinator ArticleSearchListingCoordinator { get; set; }

        public PaginatedArticleListingCoordinatorFixture()
        {
            ArticleSearchListingCoordinator = new ActiveArticleSearchListingCoordinatorFixture().Build();
        }

        public PaginatedArticleListingCoordinator Build()
        {
            return new PaginatedArticleListingCoordinator(ArticleSearchListingCoordinator);
        }
    }
}