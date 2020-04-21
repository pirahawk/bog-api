using System;
using System.Collections.Generic;

namespace Bog.Api.Domain.Models.Http
{
    public class ArticleMediaLookupResponse
    {
        public Guid ArticleId { get; set; }
        public Link[] Links { get; set; }
        public Dictionary<string, string> MediaLookup { get; set; }
    }
}