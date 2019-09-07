using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class GetLatestArticleEntryStrategy : IGetLatestArticleEntryStrategy
    {
        private readonly IBlogApiDbContext _context;

        public GetLatestArticleEntryStrategy(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<EntryContent> FindLatestEntry(Guid articleId)
        {
            await Task.CompletedTask;

            return _context.Query<Article>()
                .Where(a => a.Id == articleId)
                .SelectMany(a => a.ArticleEntries)
                .Where(ae => ae.Persisted.HasValue)
                .OrderByDescending(ae => ae.Persisted)
                .FirstOrDefault();
        }
    }
}