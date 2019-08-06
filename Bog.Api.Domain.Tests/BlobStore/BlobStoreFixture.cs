using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Values;
using Moq;
using System;

namespace Bog.Api.Domain.Tests.BlobStore
{
    public class BlobStoreFixture
    {
        private Mock<IBlobStore> _mock;
        public Mock<IBlobStore> Mock => _mock;

        public BlobStoreFixture()
        {
            _mock = new Mock<IBlobStore>();
            _mock.Setup(bs => bs.TryCreateContainer(It.IsAny<BlobStorageContainer>()))
                .ReturnsAsync(true);

            _mock.Setup(bs => bs.PersistArticleEntryAsync(It.IsAny<BlobStorageContainer>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync("http://someBlogUrl.com");

            _mock.Setup(bs => bs.PersistArticleEntryMedia(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync("http://someBlogUrl.com");
        }

        public IBlobStore Build() => _mock.Object;
    }
}