using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public interface IBogMarkdownConverterStrategy
    {
        public Task<string> GetLatestConvertedEntryContentUri(Guid articleId);
    }
}