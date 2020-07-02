using System;

namespace Bog.Api.Domain.Models.Http
{
    public class ContentResponse
    {
        public string Author { get; set; }
        public Link[] Links { get; set; }
        public bool IsPublished { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? Deleted { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] KeyWords { get; set; }
    }
}