using Bog.Api.Web.Controllers;
using Bog.Api.Web.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class MvcConfigurationExtensions
    {
        public static void WithMvc(this IServiceCollection services)
        {
            services.AddMvc(SetupMvc);
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