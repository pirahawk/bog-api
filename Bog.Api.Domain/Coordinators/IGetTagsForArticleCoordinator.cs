using System;
using System.Collections.Generic;

namespace Bog.Api.Domain.Coordinators
{
    public interface IGetTagsForArticleCoordinator
    {
        IEnumerable<String> GetAllTagsForArticle(Guid articleId);
    }
}