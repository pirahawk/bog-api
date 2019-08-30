using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class EntryMediaSearchStrategy : IEntryMediaSearchStrategy
    {
        private readonly IBlogApiDbContext _context;

        public EntryMediaSearchStrategy(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<EntryMedia> Find(Guid entryId, string mediaMD5Base64Hash)
        {
            if (mediaMD5Base64Hash == null) throw new ArgumentNullException(nameof(mediaMD5Base64Hash));

            var entryMediae = _context.Query<Article>()
                .Where(a => a.ArticleEntries.Any(ae => ae.Id == entryId))
                .SelectMany(a => a.ArticleEntries)
                .SelectMany(ae=>ae.EntryMedia);

            var matchingEntryMedia = entryMediae
                .FirstOrDefault(em => em.MD5Base64Hash == mediaMD5Base64Hash);

            return await Task.FromResult(matchingEntryMedia);
        }
    }
}