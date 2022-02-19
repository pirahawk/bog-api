using Bog.Api.Domain.BlobStore;
using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.Values;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Azure.Storage.Blobs.Models;

namespace Bog.Api.BlobStorage
{
    public class AzureBlobStore : IBlobStore
    {
        private readonly ILogger<AzureBlobStore> _logger;
        private readonly Lazy<BlobServiceClient> _blobServiceClientFactory;
        private BlobServiceClient _cloudBlobClient => _blobServiceClientFactory.Value;
        

        public AzureBlobStore(IOptionsMonitor<BlobStorageConfiguration> blobStorageOptionsAccessor, ILogger<AzureBlobStore> logger)
        {
            if (blobStorageOptionsAccessor.CurrentValue == null) throw new ArgumentNullException(nameof(blobStorageOptionsAccessor));
            _logger = logger;
            _blobServiceClientFactory = new Lazy<BlobServiceClient>(()=> TryCreateStorageAccount(blobStorageOptionsAccessor.CurrentValue.ConnectionString));
        }

        private BlobServiceClient TryCreateStorageAccount(string connectionString)
        {
            _logger.LogInformation(LogEvenIdsValueObject.BlobStorage, "attempting connect to azure blob store account");
            BlobServiceClient account = new BlobServiceClient(connectionString);
            _logger.LogInformation(LogEvenIdsValueObject.BlobStorage, "connected to azure blob store");

            return account;
        }

        public async Task<bool> TryCreateContainer(BlobStorageContainer container)
        {
            var cloudBlobContainer = GetCloudBlobContainer(container);
            var doesExist = await cloudBlobContainer.ExistsAsync();

            if (!doesExist)
            {
                var creationResponse = await cloudBlobContainer.CreateIfNotExistsAsync();
                var setAccessPolicyResponse = await cloudBlobContainer.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            return doesExist;
        }

        public async Task<string> PersistArticleEntryAsync(BlobStorageContainer container, Guid articleId, Guid entryContentId, string contentBase64)
        {
            if (string.IsNullOrWhiteSpace(contentBase64))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(contentBase64));

            var blobContainer = GetCloudBlobContainer(container);
            var textBytes = Encoding.UTF8.GetBytes(contentBase64);
            using var ms = new MemoryStream(textBytes);
            using SHA256 mySHA256 = SHA256.Create();

            var blobClient = blobContainer.GetBlobClient($"{articleId}/{entryContentId}");
            var test = blobClient.Uri.AbsoluteUri;
            await blobClient.UploadAsync(ms);

            var headers = new BlobHttpHeaders
            {
                ContentType = "text/plain",
                ContentHash = await mySHA256.ComputeHashAsync(ms)
            };
            await blobClient.SetHttpHeadersAsync(headers);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> PersistArticleEntryMedia(Guid entryMediaId, Guid entryContentId, byte[] mediaContent, string contentType)
        {
            if (mediaContent == null) throw new ArgumentNullException(nameof(mediaContent));
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentNullException(nameof(contentType));

            var entryMediaBlobContainer = GetCloudBlobContainer(BlobStorageContainer.ENTRY_MEDIA_CONTAINER);
            using var ms = new MemoryStream(mediaContent);
            using SHA256 mySHA256 = SHA256.Create();


            var blobClient = entryMediaBlobContainer.GetBlobClient($"{entryContentId}/{entryMediaId}");
            var test = blobClient.Uri.AbsoluteUri;
            await blobClient.UploadAsync(ms);

            var headers = new BlobHttpHeaders
            {
                ContentType = contentType,
                ContentHash = await mySHA256.ComputeHashAsync(ms)
            };
            await blobClient.SetHttpHeadersAsync(headers);
            await blobClient.SetHttpHeadersAsync(headers);
            return blobClient.Uri.AbsoluteUri;
        }

        private BlobContainerClient GetCloudBlobContainer(BlobStorageContainer container)
        {
            var blobName = BlobStorageLookupValueObjects.BlobNameMap[container];
            var blobContainerClient = _cloudBlobClient.GetBlobContainerClient(blobName);
            return blobContainerClient;
        }
    }
}
