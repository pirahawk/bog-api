using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Coordinators
{
    public class PaginatedArticleListingCoordinator : IPaginatedArticleListingCoordinator
    {
        private readonly IArticleSearchListingCoordinator _articleSearchListingCoordinator;

        public PaginatedArticleListingCoordinator(IArticleSearchListingCoordinator articleSearchListingCoordinator)
        {
            _articleSearchListingCoordinator = articleSearchListingCoordinator;
        }

        public async Task<IEnumerable<Article>> Find(Guid blogId, int? skip, int? take, string[] filter, string[] include)
        {

            var result = await _articleSearchListingCoordinator.Find(blogId);

            if (include?.Any() ?? false)
            {
                result = IncludeArticlesWithTags(result, include);
            }

            if (filter?.Any() ?? false)
            {
                result = ExcludeArticlesWithTags(result, filter);
            }

            result = result.OrderByDescending(a => a.Created);

            if (skip.HasValue)
            {
                result = result.Skip(skip.GetValueOrDefault());
            }

            if (take.HasValue)
            {
                result = result.Take(take.Value);
            }

            return result;
        }

        private IQueryable<Article> ExcludeArticlesWithTags(IQueryable<Article> result, string[] excludeTags)
        {
            return result.Where(article => !article.MetaTags.Any(mt => excludeTags.Contains(mt.Name)));
        }

        private IQueryable<Article> IncludeArticlesWithTags(IQueryable<Article> result, string[] includeTags)
        {
            return result.Where(article => article.MetaTags.Any(mt => includeTags.Contains(mt.Name)));
        }
    }
}