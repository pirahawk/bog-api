using Bog.Api.Domain.Tests.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
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
                var entryWithNoMatchingBlog = new ArticleRequest
                {
                    BlogId = Guid.NewGuid()
                };

                yield return new object[] { entryWithNoMatchingBlog, Enumerable.Empty<Blog>().ToArray() };


                var blog = new BlogFixture().Build();
                var entryWithNoAuthor = new ArticleRequest
                {
                    BlogId = blog.Id,
                    Author = string.Empty
                };

                yield return new object[] { entryWithNoMatchingBlog, new[]{ blog} };

            }
        }
        [Theory]
        [MemberData(nameof(ErrorTestCases))]
        public async Task ReturnsNullWhenErrorConditionsHit(ArticleRequest request, Blog[] blogs)
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

        [Fact]
        public async Task SavesNewEntryWhenConditionsMeet()
        {
            var blog = new BlogFixture().Build();
            var newEntryRequest = new ArticleRequest
            {
                BlogId = blog.Id,
                Author = "Test"
            };

            var dbContextFixture = new MockBlogApiDbContextFixture()
                .WithBlog(blog);

            var blogEntryCoordinator = new CreateBlogEntryCoordinatorFixture()
            {
                Context = dbContextFixture.Build()
            }.Build();

            var result = await blogEntryCoordinator.CreateNewEntryAsync(newEntryRequest);

            Assert.Equal(newEntryRequest.BlogId, result.BlogId);
            Assert.Equal(newEntryRequest.Author, result.Author);

            dbContextFixture.Mock.Verify(ctx => ctx.Add(result));
            dbContextFixture.Mock.Verify(ctx => ctx.SaveChanges());
        }
    }
}