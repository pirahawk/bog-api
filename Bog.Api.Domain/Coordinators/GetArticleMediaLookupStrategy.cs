using Bog.Api.Domain.Models.Article;
using System;
using System.Threading.Tasks;

namespace Bog.Api.Domain.Coordinators
{
    public class GetArticleMediaLookupStrategy : IGetArticleMediaLookupStrategy
    {
        private readonly IFindBlogArticleCoordinator _findBlogArticleCoordinator;
        private readonly IGetMediaLookupQuery _mediaLookupQuery;

        public GetArticleMediaLookupStrategy(IFindBlogArticleCoordinator findBlogArticleCoordinator, IGetMediaLookupQuery mediaLookupQuery)
        {
            _findBlogArticleCoordinator = findBlogArticleCoordinator ?? throw new ArgumentNullException(nameof(findBlogArticleCoordinator));
            _mediaLookupQuery = mediaLookupQuery ?? throw new ArgumentNullException(nameof(mediaLookupQuery));
        }


        public async Task<ArticleMediaLookup> GetMediaLookup(Guid articleId)
        {
            var article = await _findBlogArticleCoordinator.Find(articleId);

            if (article == null)
            {
                return null;
            }

            var lookup = _mediaLookupQuery.CreateMediaLookup(articleId);
            
            return new ArticleMediaLookup
            {
                MediaLookup = lookup
            };
        }
    }
}