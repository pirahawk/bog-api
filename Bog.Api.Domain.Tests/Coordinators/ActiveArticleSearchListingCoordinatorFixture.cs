using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class ActiveArticleSearchListingCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public ActiveArticleSearchListingCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public ActiveArticleSearchListingCoordinator Build()
        {
            return new ActiveArticleSearchListingCoordinator(Context);
        }
    }
}