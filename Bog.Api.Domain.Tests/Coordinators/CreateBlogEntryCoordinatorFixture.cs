using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateBlogEntryCoordinatorFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public CreateBlogEntryCoordinatorFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public CreateBlogEntryCoordinator Build()
        {
            return new CreateBlogEntryCoordinator(Context);
        }
    }
}