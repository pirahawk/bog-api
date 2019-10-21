using Bog.Api.Web.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class MvcConfigurationExtensions
    {
        public static void WithMvc(this IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //services.AddMvc(SetupMvc);
            services.AddControllers(SetupMvc);
        }

        private static void SetupMvc(MvcOptions config)
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;

            config.InputFormatters.Add(new EntryContentFormatter());
            config.InputFormatters.Add(new ArticleEntryMediaRequestFormatter());

        }
    }
}