using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Markdown
{
    public interface IBogMarkdownConverter
    {
        public Task<string> ConvertArticle(Guid articleId);
    }
}