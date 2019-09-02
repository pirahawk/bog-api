using Bog.Api.Domain.Coordinators;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategyFixture
    {
        public ICreateEntryMediaCoordinator CreateEntryMediaCoordinator { get; set; }
        public IUploadArticleEntryMediaCoordinator UploadArticleEntryMediaCoordinator { get; set; }
        public IEntryMediaSearchStrategy SearchStrategy { get; set; }

        public CreateAndPersistArticleEntryMediaStrategyFixture()
        {
            CreateEntryMediaCoordinator = new CreateEntryMediaCoordinatorFixture().Build();
            UploadArticleEntryMediaCoordinator = new UploadArticleEntryMediaCoordinatorFixture().Build();
            SearchStrategy = new EntryMediaSearchStrategyFixture().Build();
        }

        public CreateAndPersistArticleEntryMediaStrategy Build()
        {
            return new CreateAndPersistArticleEntryMediaStrategy(CreateEntryMediaCoordinator, UploadArticleEntryMediaCoordinator, SearchStrategy);
        }

    }
}