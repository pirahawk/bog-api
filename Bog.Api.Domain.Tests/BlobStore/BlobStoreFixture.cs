using System;
using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Values;
using Moq;

namespace Bog.Api.Domain.Tests.BlobStore
{
    public class BlobStoreFixture
    {
        private Mock<IBlobStore> _mock;
        public Mock<IBlobStore> Mock => _mock;

        public BlobStoreFixture()
        {
            _mock = new Mock<IBlobStore>();
            _mock.Setup(bs => bs.TryCreateContainer(It.IsAny<BlobStorageContainer>())).ReturnsAsync(true);
            _mock.Setup(bs => bs.PersistArticleEntryAsync(It.IsAny<BlobStorageContainer>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()));
        }

        public IBlobStore Build() => _mock.Object;
    }
}