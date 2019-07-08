using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public interface IDeleteArticleCoordinator
    {
        Task<bool> TryDeleteArticle(Guid articleId);
    }
}