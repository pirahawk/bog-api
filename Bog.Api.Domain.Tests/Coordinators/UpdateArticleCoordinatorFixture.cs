using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UpdateArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public UpdateArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public UpdateArticleCoordinator Build()
        {
            return new UpdateArticleCoordinator(Context);
        }
    }
}