using System;

namespace Bog.Api.Domain.Models.Http
{
    public class ArticleResponse
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        public string Author { get; set; }
        public Link[] Links { get; set; }
    }
}