using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Api.Domain.Markdown;

namespace Bog.Api.Web.Markdown
{
    public class BogMarkdownConverter : IBogMarkdownConverter
    {
        private readonly HttpClient _bogHttpClient;

        public BogMarkdownConverter(HttpClient bogHttpClient)
        {
            _bogHttpClient = bogHttpClient;
        }

        public Task<string> ConvertArticle(Guid articleId)
        {
            throw new NotImplementedException();
        }
    }
}