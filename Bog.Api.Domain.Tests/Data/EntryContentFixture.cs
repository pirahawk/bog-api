using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<EntryMedia> EntryMedia { get; set; }

        public string BlobUrl { get; set; }

        public DateTimeOffset? Persisted { get; set; }

        public EntryContentFixture()
        {
            Id = Guid.NewGuid();
            Article = new ArticleFixture().Build();
            ArticleId = Article.Id;
            Created = new MockClock().Now;
            EntryMedia = Enumerable.Empty<EntryMedia>().ToList();
            BlobUrl = "someUrl";
            Persisted = new MockClock().Now;
        }

        public EntryContent Build()
        {
            var entryContent = new EntryContent
            {
                Id = Id,
                ArticleId = ArticleId,
                Article = Article,
                Created = Created,
                EntryMedia = EntryMedia,
                Persisted = Persisted,
                BlobUrl = BlobUrl
            };

            foreach (var entryMedia in EntryMedia)
            {
                entryMedia.EntryContentId = entryContent.Id;
                entryMedia.EntryContent = entryContent;
            }

            return entryContent;
        }

        public EntryContentFixture WithMedia(params EntryMedia[] entryMedia)
        {
            EntryMedia = EntryMedia ?? Enumerable.Empty<EntryMedia>().ToList();
            EntryMedia.AddRange(entryMedia);

            return this;
        }
    }
}