using Bog.Api.Common.Tests.Time;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateArticleEntryCoordinatorFixture
    {
        public IClock Clock { get; set; }
        public IBlogApiDbContext Context { get; set; }

        public CreateArticleEntryCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
            Clock = new MockClock();
        }

        public CreateArticleEntryCoordinator Build()
        {
            return new CreateArticleEntryCoordinator(Context, Clock);
        }
    }
}