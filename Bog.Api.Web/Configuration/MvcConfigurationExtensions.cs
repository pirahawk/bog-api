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


            //services.AddCors();
            //services.AddCors(corsOpts =>
            //{
            //    corsOpts.AddPolicy("BogPolicy", builder =>
            //    {
            //        builder.AllowAnyHeader();
            //        builder.AllowAnyMethod();
            //        builder.AllowAnyOrigin();

            //    });
            //});

            services
                .AddControllers(SetupMvc)
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    opts.JsonSerializerOptions.AllowTrailingCommas = true;
                    opts.JsonSerializerOptions.IgnoreNullValues = true;

                });
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