using System;

namespace Bog.Api.Domain.Models.Http
{
    public class ArticleRequest
    {
        public Guid BlogId { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }
    }
}