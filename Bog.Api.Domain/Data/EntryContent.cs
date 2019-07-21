using System;

namespace Bog.Api.Domain.Data
{
    public class EntryContent
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}