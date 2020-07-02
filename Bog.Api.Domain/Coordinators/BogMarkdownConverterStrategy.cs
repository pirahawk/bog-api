using System;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Markdown;

namespace Bog.Api.Domain.Coordinators
{
    public class BogMarkdownConverterStrategy : IBogMarkdownConverterStrategy
    {
        private readonly IBogMarkdownConverter _bogMarkdownConverter;
        private readonly IGetLatestArticleEntryStrategy _getLatestArticleEntryStrategy;
        private readonly IBlogApiDbContext _context;
        private readonly IUploadArticleEntryCoordinator _uploadArticleEntryCoordinator;

        public BogMarkdownConverterStrategy(IBogMarkdownConverter bogMarkdownConverter, 
            IGetLatestArticleEntryStrategy getLatestArticleEntryStrategy, 
            IBlogApiDbContext context, 
            IUploadArticleEntryCoordinator uploadArticleEntryCoordinator)
        {
            _bogMarkdownConverter = bogMarkdownConverter;
            _getLatestArticleEntryStrategy = getLatestArticleEntryStrategy;
            _context = context;
            _uploadArticleEntryCoordinator = uploadArticleEntryCoordinator;
        }

        public async Task<string> GetLatestConvertedEntryContentUri(Guid articleId)
        {
           var latestEntry = await _getLatestArticleEntryStrategy.FindLatestEntry(articleId);

           if (latestEntry == null)
           {
               return string.Empty;
           }

           return !string.IsNullOrWhiteSpace(latestEntry.ConvertedBlobUrl)
               ? StringUtilities.FromBase64(latestEntry.ConvertedBlobUrl)
               : await ConvertAndPersistEntryContent(latestEntry);
        }

        private async Task<string> ConvertAndPersistEntryContent(EntryContent latestEntry)
        {
            if (!latestEntry.Persisted.HasValue || string.IsNullOrWhiteSpace(latestEntry.BlobUrl))
            {
                throw new Exception($"Entry content {latestEntry.Id} has not been persisted");
            }

            var mdContentUrl = StringUtilities.FromBase64(latestEntry.BlobUrl);
            var convertedContent = await _bogMarkdownConverter.ConvertArticle(latestEntry.ArticleId, mdContentUrl);
            var storageUrl = await _uploadArticleEntryCoordinator.UploadConvertedArticleEntry(latestEntry, convertedContent);
            await UpdateConvertedBlobUrl(latestEntry, storageUrl);

            return storageUrl;
        }

        private async Task UpdateConvertedBlobUrl(EntryContent latestEntry, string storageUrl)
        {
            latestEntry.ConvertedBlobUrl = StringUtilities.ToBase64(storageUrl);
            _context.Attach(latestEntry);
            await _context.SaveChanges();
        }
    }
}