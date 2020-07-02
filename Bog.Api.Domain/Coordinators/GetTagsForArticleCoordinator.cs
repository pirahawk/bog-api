using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class GetTagsForArticleCoordinator : IGetTagsForArticleCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public GetTagsForArticleCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetAllTagsForArticle(Guid articleId)
        {
            var tags = _context.Query<MetaTag>()
                .Where(mt => mt.ArticleId == articleId)
                .Select(mt=> mt.Name).ToArray();

            return tags;
        }
    }
}