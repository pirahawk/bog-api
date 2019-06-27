using Bog.Api.Domain.Models;
using Bog.Api.Domain.Tests.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateBlogEntryCoordinatorTest
    {
        public static IEnumerable<Object[]> ErrorTestCases
        {
            get
            {
                var entryWithNoMatchingBlog = new NewEntryRequest
                {
                    BlogId = Guid.NewGuid()
                };

                yield return new object[] { entryWithNoMatchingBlog, Enumerable.Empty<Blog>().ToArray() };


                var blog = new BlogFixture().Build();
                var entryWithNoAuthor = new NewEntryRequest
                {
                    BlogId = blog.Id,
                    Author = string.Empty
                };

                yield return new object[] { entryWithNoMatchingBlog, new[]{ blog} };

            }
        }
        [Theory]
        [MemberData(nameof(ErrorTestCases))]
        public async Task ReturnsNullWhenErrorConditionsHit(NewEntryRequest request, Blog[] blogs)
        {
            var mockBlogApiDbContextFixture = new MockBlogApiDbContextFixture();
            mockBlogApiDbContextFixture.Blogs = blogs.ToList();

            var blogApiDbContext = mockBlogApiDbContextFixture.Build();

            var blogEntryCoordinator = new CreateBlogEntryCoordinatorFixture()
            {
                Context = blogApiDbContext
            }.Build();

            Assert.Null(await blogEntryCoordinator.CreateNewEntryAsync(request));
        }
    }
}