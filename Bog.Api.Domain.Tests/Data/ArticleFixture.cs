using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<EntryContent> ArticleEntries { get; set; }

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
            ArticleEntries = Enumerable.Empty<EntryContent>().ToList();
        }

        public Article Build()
        {
            var article = new Article
            {
                Id = Id,
                Author = Author,
                Blog = Blog,
                BlogId = BlogId,
                Created = Created,
                Updated = Updated,
                Deleted = Deleted,
                IsDeleted = IsDeleted,
                IsPublished = IsPublished,
                ArticleEntries = ArticleEntries
            };

            foreach (var articleEntry in ArticleEntries)
            {
                articleEntry.ArticleId = article.Id;
                articleEntry.Article = article;
            }

            return article;
        }

        public ArticleFixture WithEntry(params EntryContent[] entries)
        {
            ArticleEntries = ArticleEntries ?? Enumerable.Empty<EntryContent>().ToList();
            ArticleEntries.AddRange(entries);
            return this;
        }
    }
}