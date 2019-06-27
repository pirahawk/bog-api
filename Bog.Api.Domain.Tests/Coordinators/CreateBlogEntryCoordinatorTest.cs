using Bog.Api.Domain.Models;
using Bog.Api.Domain.Tests.DbContext;
using System;
using Bog.Api.Domain.Data;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateBlogEntryCoordinatorTest
    {
        [Fact]
        public void ThrowsExceptionWhenBlogNotFound()
        {
            var request = new NewEntryRequest
            {
                BlogId = Guid.NewGuid()
            };

            var mockBlogApiDbContextFixture = new MockBlogApiDbContextFixture();
            var blogApiDbContext = mockBlogApiDbContextFixture.Build();

            var blogEntryCoordinator = new CreateBlogEntryCoordinatorFixture()
            {
                Context = blogApiDbContext
            }.Build();


            Assert.Empty(blogApiDbContext.Blogs);

            Assert.Null(blogEntryCoordinator.CreateNewEntry(request));
        }
    }
}