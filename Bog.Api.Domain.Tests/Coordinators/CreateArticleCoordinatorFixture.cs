using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public CreateArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public CreateArticleCoordinator Build()
        {
            return new CreateArticleCoordinator(Context);
        }
    }
}