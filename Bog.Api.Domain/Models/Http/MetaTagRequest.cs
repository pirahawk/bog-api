using System;

namespace Bog.Api.Domain.Models.Http
{
    public class MetaTagRequest
    {
        public Guid ArticleId { get; set; }
        public string Name { get; set; }
    }
}