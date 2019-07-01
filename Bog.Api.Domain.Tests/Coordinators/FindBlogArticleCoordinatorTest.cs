using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Tests.Data;
using Bog.Api.Domain.Tests.DbContext;
using Xunit;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class FindBlogArticleCoordinatorTest
    {
        public static IEnumerable<object[]> FindArticleTestCases
        {
            get
            {
                yield return new object [] { Guid.NewGuid()};

                var article = new ArticleFixture().Build();
                yield return new object[] { article.Id, article };
            }
        }

        [Theory]
        [MemberData(nameof(FindArticleTestCases))]
        public async Task ReturnsNullWhenNoArticleFound(Guid articleId, Article expectedArticle = null)
        {
            var contextFixture = new MockBlogApiDbContextFixture();
            contextFixture.Mock.Setup(m => m.Find<Article>(articleId)).Returns(Task.FromResult(expectedArticle));

            var coordinator = new FindBlogArticleCoordinatorFixture
            {
                Context = contextFixture.Build()
            }.Build();

            var result = await coordinator.Find(articleId);

            contextFixture.Mock.Verify(m => m.Find<Article>(articleId));

            if (expectedArticle == null)
            {
                Assert.Null(result);
                return;
            }

            Assert.Equal(expectedArticle, result);
        }
    }
}