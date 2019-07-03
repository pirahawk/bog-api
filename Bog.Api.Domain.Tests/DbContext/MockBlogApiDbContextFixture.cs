﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Moq;

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
            _mock.Setup(ctx => ctx.SaveChanges()).Verifiable();

            return _mock.Object;
        }

        public MockBlogApiDbContextFixture With<TEntity>(TEntity entity, params object[] keys) where TEntity : class
        {
            _mock.Setup(ctx => ctx.Find<TEntity>(keys))
                .Returns(async () => await Task.FromResult(entity))
                .Verifiable();

            return this;
        }
    }
}