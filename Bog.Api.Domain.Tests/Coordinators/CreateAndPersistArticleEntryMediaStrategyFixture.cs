using Bog.Api.Domain.Coordinators;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategyFixture
    {
        public ICreateEntryMediaCoordinator CreateEntryMediaCoordinator { get; set; }
        public IUploadArticleEntryMediaCoordinator UploadArticleEntryMediaCoordinator { get; }

        public CreateAndPersistArticleEntryMediaStrategyFixture()
        {
        }

        public CreateAndPersistArticleEntryMediaStrategy Build()
        {
            return new CreateAndPersistArticleEntryMediaStrategy(CreateEntryMediaCoordinator, UploadArticleEntryMediaCoordinator);
        }

    }
}