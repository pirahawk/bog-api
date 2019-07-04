using Bog.Api.Common.Tests.Time;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateArticleCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }
        public IClock Clock { get; set; }

        public CreateArticleCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
            Clock = new MockClock();
        }

        public CreateArticleCoordinator Build()
        {
            return new CreateArticleCoordinator(Context, Clock);
        }

    }
}