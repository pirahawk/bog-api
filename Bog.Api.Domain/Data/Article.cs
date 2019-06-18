using System;

namespace Bog.Api.Domain.Data
{
    public class Article
    {
        public Guid Id { get; set; }

        public Guid BlogId { get; set; }

        public Blog Blog { get; set; }

        public string Author { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}