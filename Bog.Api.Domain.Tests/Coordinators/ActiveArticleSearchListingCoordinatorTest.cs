using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class ActiveArticleSearchListingCoordinatorTest
    {
        public static IEnumerable<object[]> ActiveArticleTestCases
        {
            get
            {
                var publishedArticle = new ArticleFixture
                {
                    IsPublished = true,
                    IsDeleted = false
                }.Build();

                var unPublishedArticle = new ArticleFixture
                {
                    IsPublished = false,
                    IsDeleted = false
                }.Build();

                var deletedArticle = new ArticleFixture
                {
                    IsPublished = false,
                    IsDeleted = false
                }.Build();

                var blog = new BlogFixture()
                {
                    Articles = new[]{ publishedArticle , unPublishedArticle, deletedArticle }.ToList()
                }.Build();

                void AssertionFunction(IQueryable<Article> articles)
                {
                    var queryResults = articles.ToList();

                    Assert.Contains(publishedArticle, queryResults);
                    Assert.DoesNotContain(unPublishedArticle, queryResults);
                    Assert.DoesNotContain(deletedArticle, queryResults);
                }

                yield return new object[] { blog , (Action<IQueryable<Article>>) AssertionFunction };
            }
        }

        [Theory]
        [MemberData(nameof(ActiveArticleTestCases))]
        public async Task FindsActiveArticlesAsExpected(Blog blogToFind, Action<IQueryable<Article>> assertionFunction)
        {
            var mockCtx = new MockBlogApiDbContextFixture().WithQuery(new Blog[] { blogToFind });
            var articleSearchListingCoordinator = new ActiveArticleSearchListingCoordinatorFixture
            {
                Context = mockCtx.Build()
            }.Build();

            var result = await articleSearchListingCoordinator.Find(blogToFind.Id);

            assertionFunction(result);
        }
    }
}