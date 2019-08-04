using Bog.Api.Domain.Coordinators;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class CreateAndPersistArticleEntryMediaStrategyFixture
    {
        public ICreateEntryMediaCoordinator CreateEntryMediaCoordinator { get; set; }

        public CreateAndPersistArticleEntryMediaStrategyFixture()
        {
            //CreateEntryMediaCoordinator = new CreateEntryMediaCoordinatorFixture().Build();
        }

        public CreateAndPersistArticleEntryMediaStrategy Build()
        {
            return new CreateAndPersistArticleEntryMediaStrategy(CreateEntryMediaCoordinator);
        }

    }
}