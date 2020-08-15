using Bog.Api.Web.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Bog.Api.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .WithBlogApiConfigurationJson()
                .WithKeyVaultConfiguration()
                .UseStartup<Startup>();
    }
}
