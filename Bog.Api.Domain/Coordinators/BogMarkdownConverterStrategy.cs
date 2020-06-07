using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Markdown;

namespace Bog.Api.Domain.Coordinators
{
    public class BogMarkdownConverterStrategy : IBogMarkdownConverterStrategy
    {
        private IBogMarkdownConverter _bogMarkdownConverter;
        private IGetLatestArticleEntryStrategy _getLatestArticleEntryStrategy;

        public BogMarkdownConverterStrategy(IBogMarkdownConverter bogMarkdownConverter, IGetLatestArticleEntryStrategy getLatestArticleEntryStrategy)
        {
            _bogMarkdownConverter = bogMarkdownConverter;
            _getLatestArticleEntryStrategy = getLatestArticleEntryStrategy;
        }
        public async Task<string> GetLatestConvertedEntryContentUri(Guid articleId)
        {
           var latestEntry = await _getLatestArticleEntryStrategy.FindLatestEntry(articleId);

           if (latestEntry == null)
           {
               return string.Empty;
           }

           //TODO implement this after model change
           return string.Empty;
        }
    }
}