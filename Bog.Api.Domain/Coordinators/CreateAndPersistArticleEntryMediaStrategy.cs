using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategy : ICreateAndPersistArticleEntryMediaStrategy
    {
        private readonly ICreateEntryMediaCoordinator _createEntryMediaCoordinator;

        public CreateAndPersistArticleEntryMediaStrategy(ICreateEntryMediaCoordinator createEntryMediaCoordinator)
        {
            _createEntryMediaCoordinator = createEntryMediaCoordinator;
        }
        public async Task<EntryMedia> PersistArticleEntryMediaAsync(ArticleEntryMediaRequest entryMediaRequest)
        {
            var articleEntry = await _createEntryMediaCoordinator.CreateArticleEntryMedia(entryMediaRequest);
            return articleEntry;
        }
    }
}