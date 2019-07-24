using Bog.Api.Domain.Coordinators;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateAndPersistArticleEntryStrategyFixture
    {
        public IUploadArticleEntryCoordinator UploadCoordinator { get; set; }
        public ICreateArticleEntryCoordinator CreateEntryCoordinator { get; set; }

        public CreateAndPersistArticleEntryStrategyFixture()
        {
            CreateEntryCoordinator = new CreateArticleEntryCoordinatorFixture().Build();
            UploadCoordinator = new UploadArticleEntryCoordinatorFixture().Build();
        }

        public CreateAndPersistArticleEntryStrategy Build()
        {
            return new CreateAndPersistArticleEntryStrategy(CreateEntryCoordinator, UploadCoordinator);
        }
    }
}