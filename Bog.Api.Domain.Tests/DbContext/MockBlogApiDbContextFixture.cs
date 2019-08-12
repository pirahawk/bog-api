using Bog.Api.Domain.DbContext;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Bog.Api.Domain.Tests.DbContext
{
    public class MockBlogApiDbContextFixture
    {
        private Mock<IBlogApiDbContext> _mock;
        public Mock<IBlogApiDbContext> Mock => _mock;


        public MockBlogApiDbContextFixture()
        {
            _mock = new Mock<IBlogApiDbContext>();
        }

        public IBlogApiDbContext Build()
        {
            _mock.Setup(ctx => ctx.Add(It.IsAny<object>())).Verifiable();
            _mock.Setup(ctx => ctx.Attach(It.IsAny<object>())).Verifiable();
            _mock.Setup(ctx => ctx.SaveChanges()).Verifiable();

            return _mock.Object;
        }

        public MockBlogApiDbContextFixture With<TEntity>(TEntity entity, params object[] keys) where TEntity : class
        {
            _mock.Setup(ctx => ctx.Find<TEntity>(keys))
                .ReturnsAsync(entity)
                .Verifiable();

            return this;
        }

        public MockBlogApiDbContextFixture WithQuery<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _mock.Setup(ctx => ctx.Query<TEntity>(It.IsAny<string[]>()))
                .Returns(entities.AsQueryable);

            return this;
        }
    }
}