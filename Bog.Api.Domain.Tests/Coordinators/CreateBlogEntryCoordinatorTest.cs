using Bog.Api.Domain.Models;
using Bog.Api.Domain.Tests.DbContext;
using System;
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
            var blogEntryCoordinator = new CreateBlogEntryCoordinatorFixture()
            {
                Context = mockBlogApiDbContextFixture.Build()
            }.Build();

            blogEntryCoordinator.CreateNewEntry(request);
        }
    }
}