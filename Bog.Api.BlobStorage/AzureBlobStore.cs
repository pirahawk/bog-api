using System;
using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Values;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bog.Api.BlobStorage
{
    public class AzureBlobStore : IBlobStore
    {
        private readonly ILogger<AzureBlobStore> _logger;
        private readonly Lazy<CloudStorageAccount> _storageAccountProvider;
        private CloudStorageAccount _cloudStorageAccount => _storageAccountProvider.Value;

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

    }
}
