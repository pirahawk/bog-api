using System.Linq;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Tests.DbContext;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class GetMediaLookupQueryFixture
    {
        public IBlogApiDbContext Context { get; set; }

        public GetMediaLookupQueryFixture()
        {
            Context = new MockBlogApiDbContextFixture()
                .WithQuery(Enumerable.Empty<Article>().AsQueryable())
                .Build();
        }
        public GetMediaLookupQuery Build()
        {
            return new GetMediaLookupQuery(Context);
        }
    }
}