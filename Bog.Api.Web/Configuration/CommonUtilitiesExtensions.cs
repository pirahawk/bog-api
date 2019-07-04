using Bog.Api.Common.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class CommonUtilitiesExtensions
    {
        public static void WithUtilities(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();
        }
    }
}