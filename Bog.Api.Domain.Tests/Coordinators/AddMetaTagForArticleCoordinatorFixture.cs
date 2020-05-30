using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class AddMetaTagForArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public AddMetaTagForArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public AddMetaTagForArticleCoordinator Build()
        {
            return new AddMetaTagForArticleCoordinator(Context);
        }
    }
}