using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class EntryMediaSearchStrategyFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public EntryMediaSearchStrategyFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public EntryMediaSearchStrategy Build()
        {
            return new EntryMediaSearchStrategy(Context);
        }
    }
}