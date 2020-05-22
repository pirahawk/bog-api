using System;

namespace Bog.Api.Domain.Data
{
    public class MetaTag
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public string Name { get; set; }
    }
}