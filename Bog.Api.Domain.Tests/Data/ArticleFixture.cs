using System;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Tests.Data
{
    public class ArticleFixture
    {
        public DateTimeOffset Created { get; set; }

        public string Author { get; set; }

        public Blog Blog { get; set; }

        public Guid BlogId { get; set; }

        public Guid Id { get; set; }

        public ArticleFixture()
        {
            var blogFixture = new BlogFixture();

            this.Id = Guid.NewGuid();
            this.Blog = blogFixture.Build();
            this.BlogId = blogFixture.Id;
            this.Author = "test";
            this.Created = DateTimeOffset.UtcNow;
        }

        public Article Build()
        {
            return new Article
            {
                Id = Id,
                Created = Created,
                Author = Author,
                Blog = Blog,
                BlogId = BlogId
            };
        }

    }
}