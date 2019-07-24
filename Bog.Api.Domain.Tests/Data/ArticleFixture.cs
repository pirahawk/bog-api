using System;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Tests.Data
{
    public class ArticleFixture
    {

        public Guid Id { get; set; }

        public Blog Blog { get; set; }

        public Guid BlogId { get; set; }

        public string Author { get; set; }

        public bool IsPublished { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        public ArticleFixture()
        {
            var blogFixture = new BlogFixture();

            Id = Guid.NewGuid();
            Blog = blogFixture.Build();
            BlogId = blogFixture.Id;
            Author = "test";
            IsPublished = false;
            Created = DateTimeOffset.UtcNow;
            IsDeleted = false;
        }

        public Article Build()
        {
            return new Article
            {
                Id = Id,
                Author = Author,
                Blog = Blog,
                BlogId = BlogId,
                Created = Created,
                Updated = Updated,
                Deleted = Deleted,
                IsDeleted = IsDeleted,
                IsPublished = IsPublished
            };
        }
    }
}