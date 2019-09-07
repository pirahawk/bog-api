using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class GetLatestArticleEntryStrategyFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public GetLatestArticleEntryStrategyFixture()
        {
            Context = new MockBlogApiDbContextFixture().Build();
        }

        public GetLatestArticleEntryStrategy Build()
        {
            return new GetLatestArticleEntryStrategy(Context);
        }
    }
}