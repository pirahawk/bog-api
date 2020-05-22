using System;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Tests.Data
{
    public class MetaTagFixture
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Article Article { get; set; }
        public Guid ArticleId { get; set; }

        public MetaTagFixture()
        {
            Id = Guid.NewGuid();
            Name = "SomeMetaTag";
            Article = new ArticleFixture().Build();
            ArticleId = Article.Id;
        }

        public MetaTag Build()
        {
            return new MetaTag
            {
                Id = Id,
                ArticleId = ArticleId,
                Article = Article,
                Name = Name
            };
        }

    }
}