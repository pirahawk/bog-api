﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Tests.Data
{
    public class ArticleFixture
    {
        public Guid Id { get; set; }

        public Blog Blog { get; set; }

        public Guid BlogId { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        public List<EntryContent> ArticleEntries { get; set; }

        public List<MetaTag> MetaTags { get; set; }

        public ArticleFixture()
        {
            var blogFixture = new BlogFixture();

            Id = Guid.NewGuid();
            Blog = blogFixture.Build();
            BlogId = blogFixture.Id;
            Author = "test";
            Title = "some-post";
            Description = "some post about something";
            IsPublished = false;
            Created = DateTimeOffset.UtcNow;
            IsDeleted = false;
            ArticleEntries = Enumerable.Empty<EntryContent>().ToList();
            MetaTags = Enumerable.Empty<MetaTag>().ToList();
        }

        public Article Build()
        {
            var article = new Article
            {
                Id = Id,
                Author = Author,
                Title = Title,
                Description = Description,
                Blog = Blog,
                BlogId = BlogId,
                Created = Created,
                Updated = Updated,
                Deleted = Deleted,
                IsDeleted = IsDeleted,
                IsPublished = IsPublished,
                ArticleEntries = ArticleEntries,
                MetaTags = MetaTags
            };

            foreach (var articleEntry in ArticleEntries)
            {
                articleEntry.ArticleId = article.Id;
                articleEntry.Article = article;
            }

            foreach (var metaTag in MetaTags)
            {
                metaTag.ArticleId = article.Id;
                metaTag.Article = article;
            }

            return article;
        }

        public ArticleFixture WithEntry(params EntryContent[] entries)
        {
            ArticleEntries ??= Enumerable.Empty<EntryContent>().ToList();
            ArticleEntries.AddRange(entries);
            return this;
        }

        public ArticleFixture WithTags(params MetaTagRequest[] tagsToAdd)
        {
            MetaTags = tagsToAdd
                .Select(t => new MetaTagFixture {Name = t.Name}.Build())
                .ToList();
            return this;
        }
    }
}