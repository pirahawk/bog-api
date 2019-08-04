using Bog.Api.Common.Tests.Time;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateEntryMediaCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }
        public IClock Clock { get; set; }

        public CreateEntryMediaCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
            Clock = new MockClock();
        }

        public CreateEntryMediaCoordinator Build()
        {
            return new CreateEntryMediaCoordinator(Context, Clock);
        }
    }
}