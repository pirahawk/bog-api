using System;

namespace Bog.Api.Domain.Models.Http
{
    public class ArticleEntryResponse
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public DateTimeOffset Created { get; set; }

        public Link[] Links { get; set; }

    }
}