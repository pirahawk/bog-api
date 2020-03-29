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
            if (string.IsNullOrWhiteSpace(mediaMD5Base64Hash))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mediaMD5Base64Hash));

            var entryMedia = GetAllMediaUploadedForArticle(entryId);
            var matchingEntryMedia = entryMedia.FirstOrDefault(em => em.MD5Base64Hash == mediaMD5Base64Hash);

            return await Task.FromResult(matchingEntryMedia);
        }

        public async Task<EntryMedia> Find(Guid entryId, string mediaMd5Base64Hash, string mediaFileName)
        {
            if (string.IsNullOrWhiteSpace(mediaMd5Base64Hash))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mediaMd5Base64Hash));
            if (string.IsNullOrWhiteSpace(mediaFileName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mediaFileName));

            var entryMedia = GetAllMediaUploadedForArticle(entryId);
            var matchingEntryMedia = entryMedia.FirstOrDefault(em => em.MD5Base64Hash.Equals(mediaMd5Base64Hash) && em.FileName.Equals(mediaFileName));

            return await Task.FromResult(matchingEntryMedia);
        }

        private IQueryable<EntryMedia> GetAllMediaUploadedForArticle(Guid entryId)
        {
            return _context.Query<Article>()
                .Where(a => a.ArticleEntries.Any(ae => ae.Id == entryId))
                .SelectMany(a => a.ArticleEntries)
                .SelectMany(ae => ae.EntryMedia);
        }
    }
}