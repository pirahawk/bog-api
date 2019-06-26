using System.Collections.Generic;
using System.Linq;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Moq;

namespace Bog.Api.Domain.Tests.DbContext
{
    public class MockBlogApiDbContextFixture
    {
        private Mock<IBlogApiDbContext> _mock;

        public Mock<IBlogApiDbContext> Mock
        {
            get => _mock;
        }

        public List<Blog> Blogs { get; }


        public MockBlogApiDbContextFixture()
        {
            this.Blogs = new List<Blog>();
        }

        public IBlogApiDbContext Build()
        {
            _mock = new Mock<IBlogApiDbContext>();
            _mock.Setup(ctx => ctx.Blogs).Returns(Blogs.AsQueryable());

            return _mock.Object;
        }
    }
}