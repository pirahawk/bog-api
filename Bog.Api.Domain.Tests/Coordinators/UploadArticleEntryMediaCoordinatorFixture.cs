﻿using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Coordinators;
using Bog.Api.Domain.Tests.BlobStore;

namespace Bog.Api.Domain.Tests.Coordinators
{
    public class UploadArticleEntryMediaCoordinatorFixture
    {
        public IBlobStore BlobStore { get; set; }

        public UploadArticleEntryMediaCoordinatorFixture()
        {
            BlobStore = new BlobStoreFixture().Build();
        }

        public UploadArticleEntryMediaCoordinator Build()
        {
            return new UploadArticleEntryMediaCoordinator(BlobStore);
        }
    }
}