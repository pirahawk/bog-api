using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Tests.BlobStore;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UploadArticleEntryCoordinatorFixture
    {
        public IBlobStore BlobStore { get; set; }

        public UploadArticleEntryCoordinatorFixture()
        {
            BlobStore = new BlobStoreFixture().Build();
        }
        public UploadArticleEntryCoordinator Build()
        {
            return new UploadArticleEntryCoordinator(BlobStore);
        }
    }
}