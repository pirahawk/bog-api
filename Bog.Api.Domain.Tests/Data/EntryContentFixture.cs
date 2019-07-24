using System;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Tests.Data
{
    public class EntryContentFixture
    {
        public DateTimeOffset Created { get; set; }

        public Article Article { get; set; }

        public Guid ArticleId { get; set; }

        public Guid Id { get; set; }

        public EntryContentFixture()
        {
            Id = Guid.NewGuid();
            Article = new ArticleFixture().Build();
            ArticleId = Article.Id;
            Created = new MockClock().Now;
        }

        public EntryContent Build()
        {
            return new EntryContent
            {
                Id = Id,
                ArticleId = ArticleId,
                Article = Article,
                Created = Created
            };
        }
    }
}