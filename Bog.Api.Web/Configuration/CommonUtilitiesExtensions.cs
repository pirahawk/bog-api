using Bog.Api.BlobStorage;
using Bog.Api.Common.Time;
using Bog.Api.Db.DbContexts;
using Bog.Api.Domain.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class CommonUtilitiesExtensions
    {
        public static void WithUtilities(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
        }

        public static void WithCloudUtilities(this IServiceCollection services)
        {
            services.AddTransient<IBlobStore, AzureBlobStore>();
        }

        public static void WithEFDbContext(this IServiceCollection services)
        {
            services.AddDbContext<BlogApiDbContext>();
            services.AddTransient<IBlogApiDbContext, BlogApiDbContextAdapter>();
        }
    }
}