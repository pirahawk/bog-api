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
        public Mock<IBlogApiDbContext> Mock => _mock;

        public List<Blog> Blogs { get; set; }

        public MockBlogApiDbContextFixture()
        {
            this.Blogs = new List<Blog>();
            _mock = new Mock<IBlogApiDbContext>();
        }

        public IBlogApiDbContext Build()
        {
            
            _mock.Setup(ctx => ctx.Add(It.IsAny<object>())).Verifiable();
            _mock.Setup(ctx => ctx.SaveChanges()).Verifiable();
            _mock.Setup(ctx => ctx.Blogs).Returns(Blogs.AsQueryable());

            return _mock.Object;
        }

        public MockBlogApiDbContextFixture WithBlog(Blog blog)
        {
            Blogs.Add(blog);
            return this;
        }
    }
}