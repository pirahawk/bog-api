using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.Values;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bog.Api.BlobStorage
{
    public class AzureBlobStore : IBlobStore
    {
        private readonly ILogger<AzureBlobStore> _logger;
        private readonly Lazy<CloudStorageAccount> _storageAccountProvider;
        private CloudStorageAccount _cloudStorageAccount => _storageAccountProvider.Value;
        private CloudBlobClient _cloudBlobClient => _cloudStorageAccount.CreateCloudBlobClient();
        private readonly IDictionary<BlobStorageContainer, string> _blobNameLookup = new Dictionary<BlobStorageContainer, string>()
        {
            {BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER, BlobStorageValueObjects.MARKDOWN_ARTICLE_ENTRIES_CONTAINER},
            {BlobStorageContainer.TRANSLATED_ARTICLE_ENTRIES_CONTAINER, BlobStorageValueObjects.TRANSLATED_ARTICLE_ENTRIES_CONTAINER}
        };

        public AzureBlobStore(IOptionsMonitor<BlobStorageConfiguration> blobStorageOptionsAccessor, ILogger<AzureBlobStore> logger)
        {
            if (blobStorageOptionsAccessor.CurrentValue == null) throw new ArgumentNullException(nameof(blobStorageOptionsAccessor));
            _logger = logger;
            _storageAccountProvider = new Lazy<CloudStorageAccount>(()=> TryCreateStorageAccount(blobStorageOptionsAccessor.CurrentValue.ConnectionString));
        }

        private CloudStorageAccount TryCreateStorageAccount(string connectionString)
        {
            _logger.LogInformation(LogEvenIdsValueObject.BlobStorage, "attempting connect to azure blob store account");

            CloudStorageAccount account;

            var createAttempt = CloudStorageAccount.TryParse(connectionString, out account);

            if (!createAttempt)
            {
                _logger.LogError(LogEvenIdsValueObject.BlobStorage, "could not connect to azure blob store container");
                throw new Exception("could not connect to azure blob store container");
            }

            _logger.LogInformation(LogEvenIdsValueObject.BlobStorage, "connected to azure blob store");

            return account;
        }

        public async Task<bool> TryCreateContainer(BlobStorageContainer container)
        {
            var cloudBlobContainer = GetCloudBlobContainer(container);
            var isNewlyCreated = await cloudBlobContainer.CreateIfNotExistsAsync();

            if (isNewlyCreated)
            {
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob,
                };
                await cloudBlobContainer.SetPermissionsAsync(permissions);
            }

            return isNewlyCreated;
        }

        public async Task PersistArticleEntryAsync(BlobStorageContainer container, Guid articleId, Guid entryContentId, string contentBase64)
        {
            if (string.IsNullOrWhiteSpace(contentBase64))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(contentBase64));

            var cloudBlobContainer = GetCloudBlobContainer(container);
            var cloudBlobDirectory = cloudBlobContainer.GetDirectoryReference($"{articleId}");
            var blockBlobReference = cloudBlobDirectory.GetBlockBlobReference($"{entryContentId}");
            await blockBlobReference.UploadTextAsync(contentBase64);
        }

        private CloudBlobContainer GetCloudBlobContainer(BlobStorageContainer container)
        {
            var blobName = _blobNameLookup[container];
            return _cloudBlobClient.GetContainerReference(blobName);
        }
    }
}
