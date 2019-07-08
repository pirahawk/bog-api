using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class FindBlogArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public FindBlogArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public FindBlogArticleCoordinator Build()
        {
            return new FindBlogArticleCoordinator(Context);
        }
    }
}