using Bog.Api.Domain.Tests.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Tests.Data;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateArticleCoordinatorTest
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
                    Author = string.Empty,
                    Title = "Some-title"
                };

                yield return new object[] { entryWithNoAuthor, new[]{ blog} };

                var entryWithNoTitle = new ArticleRequest
                {
                    BlogId = blog.Id,
                    Author = "Some-author",
                    Title = string.Empty
                };

                yield return new object[] { entryWithNoTitle, new[] { blog } };

            }
        }
        [Theory]
        [MemberData(nameof(ErrorTestCases))]
        public async Task ReturnsNullWhenErrorConditionsHit(ArticleRequest request, Blog[] blogs)
        {
            var mockBlogApiDbContextFixture = new MockBlogApiDbContextFixture();

            foreach (var blog in blogs)
            {
                mockBlogApiDbContextFixture.With(blog, blog.Id);
            }

            var blogApiDbContext = mockBlogApiDbContextFixture.Build();

            var blogEntryCoordinator = new CreateArticleCoordinatorFixture()
            {
                Context = blogApiDbContext
            }.Build();

            Assert.Null(await blogEntryCoordinator.CreateNewArticleAsync(request));
        }

        [Fact]
        public async Task SavesNewEntryWhenConditionsMeet()
        {
            var blog = new BlogFixture().Build();
            var newEntryRequest = new ArticleRequest
            {
                BlogId = blog.Id,
                Author = "Test",
                Title = "Some-title",
                Description = "some-Description",
            };

            var dbContextFixture = new MockBlogApiDbContextFixture().With(blog, blog.Id);
            var clock = new MockClock();

            var blogEntryCoordinator = new CreateArticleCoordinatorFixture()
            {
                Context = dbContextFixture.Build(),
                Clock = clock
            }.Build();

            var result = await blogEntryCoordinator.CreateNewArticleAsync(newEntryRequest);

            Assert.Equal(newEntryRequest.BlogId, result.BlogId);
            Assert.Equal(newEntryRequest.Author, result.Author);
            Assert.Equal(newEntryRequest.Title, result.Title);
            Assert.Equal(newEntryRequest.Description, result.Description);
            Assert.Equal(clock.Now, result.Created);

            dbContextFixture.Mock.Verify(ctx => ctx.Add(result));
            dbContextFixture.Mock.Verify(ctx => ctx.SaveChanges());
        }
    }
}