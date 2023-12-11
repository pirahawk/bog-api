//using Bog.Api.Web.Configuration;
//using Microsoft.AspNetCore;
//using Microsoft.AspNetCore.Hosting;

//namespace Bog.Api.Web
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateWebHostBuilder(args).Build().Run();
//        }

//        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
//            WebHost.CreateDefaultBuilder(args)
//                .WithBlogApiConfigurationJson()
//                .WithKeyVaultConfiguration()
//                .UseStartup<Startup>();
//    }
//}

using Bog.Api.Web.Configuration;
using Microsoft.OpenApi.Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.WithApiConfiguration();
builder.WithMvcControllersAndFormatters();
builder.WithMarkdownConverterHttpClient();

builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CheckoutChallenge.Payment.Api",
        Description = "Api representing an acquiring Payment service",
        Version = "v1"
    });
    c.EnableAnnotations();
});


var app = builder.Build();

app.UseRouting();
app.MapHealthChecks("/health");
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bog API");
    });
}


app.Run();