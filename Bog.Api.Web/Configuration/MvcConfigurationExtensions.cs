using Bog.Api.Web.Formatters;

namespace Bog.Api.Web.Configuration
{
    public static class MvcConfigurationExtensions
    {
        public static void WithMvcControllersAndFormatters(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers(mvcOptions =>
            {
                mvcOptions.RespectBrowserAcceptHeader = true;
                mvcOptions.ReturnHttpNotAcceptable = true;

                mvcOptions.InputFormatters.Insert(0, new EntryContentFormatter());
                mvcOptions.InputFormatters.Insert(0, new ArticleEntryMediaRequestFormatter());
            });
        }
    }
}


// I don't know if i need this

    //public static void WithMvc(this IServiceCollection services)
    //{
    //    // If using Kestrel:
    //    services.Configure<KestrelServerOptions>(options =>
    //    {
    //        options.AllowSynchronousIO = true;

    //    });

    //    // If using IIS:
    //    services.Configure<IISServerOptions>(options =>
    //    {
    //        options.AllowSynchronousIO = true;
    //    });


    //    //services.AddCors();
    //    //services.AddCors(corsOpts =>
    //    //{
    //    //    corsOpts.AddPolicy("BogPolicy", builder =>
    //    //    {
    //    //        builder.AllowAnyHeader();
    //    //        builder.AllowAnyMethod();
    //    //        builder.AllowAnyOrigin();

    //    //    });
    //    //});



    //    //services
    //    //    .AddControllers(SetupMvc)
    //    //    .AddJsonOptions(opts =>
    //    //    {
    //    //        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    //    //        opts.JsonSerializerOptions.AllowTrailingCommas = true;
    //    //        opts.JsonSerializerOptions.IgnoreNullValues = true;

    //    //    });


    //    services.AddMvc(SetupMvc);
    //}
