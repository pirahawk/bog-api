using Bog.Api.Common.Time;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models.Http;
using System;
using System.Threading.Tasks;
using Bog.Api.Common;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateEntryMediaCoordinator : ICreateEntryMediaCoordinator
    {
        private readonly IBlogApiDbContext _context;
        private readonly IClock _clock;

        public CreateEntryMediaCoordinator(IBlogApiDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task<EntryMedia> CreateArticleEntryMedia(ArticleEntryMediaRequest entryMediaRequest)
        {
            if (entryMediaRequest == null) throw new ArgumentNullException(nameof(entryMediaRequest));

            var entryContent = await _context.Find<EntryContent>(entryMediaRequest.EntryId);

            if (entryContent == null)
            {
                return null;
            }

            return await CreateNewEntryMediaFor(entryContent, entryMediaRequest);
        }

        public async Task<EntryMedia> MarkUploadedSuccess(EntryMedia articleEntryMedia, string uploadUri)
        {
            if (articleEntryMedia == null) throw new ArgumentNullException(nameof(articleEntryMedia));
            if (string.IsNullOrWhiteSpace(uploadUri)) throw new ArgumentNullException(nameof(uploadUri));

            _context.Attach(articleEntryMedia);

            articleEntryMedia.BlobUrl = StringUtilities.ToBase64(uploadUri);
            articleEntryMedia.Persisted = _clock.Now;

            await _context.SaveChanges();

            return articleEntryMedia;
        }

        private async Task<EntryMedia> CreateNewEntryMediaFor(EntryContent entryContent, ArticleEntryMediaRequest entryMediaRequest)
        {
            var entryMedia = new EntryMedia();
            entryMedia.FileName = entryMediaRequest.FileName;
            entryMedia.ContentType = entryMediaRequest.ContentType;
            entryMedia.BlobFileName = Guid.NewGuid();
            entryMedia.EntryContentId = entryContent.Id;
            entryMedia.MD5Base64Hash = entryMediaRequest.MD5Base64Hash;
            entryMedia.Created = _clock.Now;

            await _context.Add(entryMedia);
            await _context.SaveChanges();

            return entryMedia;
        }
    }
}