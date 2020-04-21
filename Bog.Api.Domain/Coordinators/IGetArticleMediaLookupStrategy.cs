using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Models.Article;

namespace Bog.Api.Domain.Coordinators
{
    public interface IGetArticleMediaLookupStrategy
    {
        Task<ArticleMediaLookup> GetMediaLookup(Guid articleId);
    }
}