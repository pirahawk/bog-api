using System;
using System.Collections.Generic;

namespace Bog.Api.Domain.Data
{
    public class Blog
    {
        public Guid Id { get; set; }

        public IEnumerable<Article> Articles { get; set; }
    }
}