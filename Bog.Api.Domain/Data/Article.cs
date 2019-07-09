using System;
using System.Collections.Generic;

namespace Bog.Api.Domain.Data
{
    public class Article
    {
        public Guid Id { get; set; }

        public Guid BlogId { get; set; }

        public Blog Blog { get; set; }

        public string Author { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        public IEnumerable<EntryContent> ArticleEntries { get; set; }
    }
}