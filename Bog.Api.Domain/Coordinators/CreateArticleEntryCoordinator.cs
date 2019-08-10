using System;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Common.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateArticleEntryCoordinator : ICreateArticleEntryCoordinator
    {
        private readonly IBlogApiDbContext _context;
        private readonly IClock _clock;

        public CreateArticleEntryCoordinator(IBlogApiDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task<EntryContent> CreateArticleEntry(Guid articleId, ArticleEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            var article = await _context.Find<Article>(articleId);

            if (article == null)
            {
                return null;
            }

            return await CreateNewEntryFor(article, entry);
        }

        public async Task<EntryContent> MarkUploadSuccess(EntryContent entryContent, string uploadUrl)
        {
            if (entryContent == null) throw new ArgumentNullException(nameof(entryContent));
            if (string.IsNullOrWhiteSpace(uploadUrl)) throw new ArgumentNullException(nameof(uploadUrl));

            _context.Attach(entryContent);

            entryContent.Persisted = _clock.Now;
            entryContent.BlobUrl = StringUtilities.ToBase64(uploadUrl);

            await _context.SaveChanges();

            return entryContent;
        }

        private async Task<EntryContent> CreateNewEntryFor(Article article, ArticleEntry entry)
        {
            var entryContent = new EntryContent
            {
                ArticleId = article.Id,
                Created = _clock.Now,
            };

            await _context.Add(entryContent);
            await _context.SaveChanges();

            return entryContent;
        }
    }
}