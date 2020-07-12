using Bog.Api.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class ApiConfigurationExtensions
    {
        public static void WithApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EntityConfiguration>(configuration.GetSection("ConnectionStrings"));
            services.Configure<BlobStorageConfiguration>(configuration.GetSection("BlobStorageConfiguration"));
            services.Configure<MarkdownConverterConfiguration>(configuration.GetSection("MarkdownConverterConfiguration"));
            services.Configure<BlogApiSettings>(configuration.GetSection("BlogApiSettings"));
        }
    }
}