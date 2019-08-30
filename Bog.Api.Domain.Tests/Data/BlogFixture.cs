using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Tests.Data
{
    public class BlogFixture
    {
        public List<Article> Articles { get; set; }
        public Guid Id { get; set; }

        public BlogFixture()
        {
            this.Id = Guid.NewGuid();
            this.Articles = Enumerable.Empty<Article>().ToList();
        }

        public Blog Build()
        {
            var blog = new Blog
            {
                Id   = Id,
                Articles = Articles
            };

            foreach (var article in Articles)
            {
                article.BlogId = blog.Id;
                article.Blog = blog;
            }
            return blog;
        }
    }
}