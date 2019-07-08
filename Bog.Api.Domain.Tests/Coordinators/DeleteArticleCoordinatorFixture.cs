using Bog.Api.Common.Tests.Time;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class DeleteArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }
        public IClock Clock { get; set; }

        public DeleteArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
            Clock = new MockClock();
        }

        public DeleteArticleCoordinator Build()
        {
            return new DeleteArticleCoordinator(Context, Clock);
        }
    }
}