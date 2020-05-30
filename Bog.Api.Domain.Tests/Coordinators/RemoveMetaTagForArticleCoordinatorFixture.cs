using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class RemoveMetaTagForArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public RemoveMetaTagForArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public RemoveMetaTagForArticleCoordinator Build()
        {
            return new RemoveMetaTagForArticleCoordinator(Context);
        }
    }
}