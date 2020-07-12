using System;

namespace Bog.Api.Domain.Configuration
{
    public class BlogApiSettings
    {
        public const long MAX_ENTRY_REQUEST_LIMIT_BYTES = 20971520;
        public Guid Api { get; set; }
    }
}