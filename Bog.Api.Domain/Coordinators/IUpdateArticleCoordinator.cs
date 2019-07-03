using System;
using Bog.Api.Domain.Models.Http;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public interface IUpdateArticleCoordinator
    {
        Task<bool> TryUpdateArticle(Guid articleId, ArticleRequest updatedArticle);
    }
}