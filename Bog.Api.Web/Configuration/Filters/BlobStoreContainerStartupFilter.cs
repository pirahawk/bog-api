using Bog.Api.Common.Time;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Bog.Api.Domain.BlobStore;

namespace Bog.Api.Web.Configuration.Filters
{
    public class BlobStoreContainerStartupFilter: IStartupFilter
    {
        private readonly ILogger<BlogDbContextStartupDataSeeder> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IClock _clock;
        

        public BlobStoreContainerStartupFilter(ILogger<BlogDbContextStartupDataSeeder> logger, IWebHostEnvironment env, IClock clock)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env;
            _clock = clock;
        }
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (builder) =>
            {
                _logger.LogInformation(LogEvenIdsValueObject.BlobStorage, "Configuring Azure Blob Storage containers");
                Task.WaitAll(CreateRootBlobContainers(builder));
                next(builder);
            };
        }

        public async Task CreateRootBlobContainers(IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.CreateScope())
            {
                var blobStore = serviceScope.ServiceProvider.GetService<IBlobStore>();

                foreach (var blobStorageContainer in BlobStorageLookupValueObjects.BlobNameMap.Keys)
                {
                    await blobStore.TryCreateContainer(blobStorageContainer);
                    _logger.LogInformation(LogEvenIdsValueObject.BlobStorage, $"Created Container: {BlobStorageLookupValueObjects.BlobNameMap[blobStorageContainer]}");
                }
            }
        }
    }
}