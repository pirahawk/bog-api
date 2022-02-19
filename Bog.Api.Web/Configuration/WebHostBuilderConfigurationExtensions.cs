using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Linq;
//using Microsoft.Azure.KeyVault;
//using Microsoft.Azure.Services.AppAuthentication;
//using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace Bog.Api.Web.Configuration
{
    public static class WebHostBuilderConfigurationExtensions
    {
        public static IWebHostBuilder WithBlogApiConfigurationJson(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(AddConfigurationJsonFiles);
            return builder;
        }

        public static IWebHostBuilder WithKeyVaultConfiguration(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(AddKeyVaultConfiguration);
            return builder;
        }

        private static void AddKeyVaultConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            if (!context.HostingEnvironment.IsProduction())
            {
                return;
            }

            var config = builder.Build();
            var keyVaultName = config.GetValue<string>("AzKeyVault");

            //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //var keyVaultClient = new KeyVaultClient(
            //    new KeyVaultClient.AuthenticationCallback(
            //        azureServiceTokenProvider.KeyVaultTokenCallback));

            //builder.AddAzureKeyVault(
            //    $"https://{keyVaultName}.vault.azure.net/",
            //    keyVaultClient,
            //    new DefaultKeyVaultSecretManager());
        }

        private static void AddConfigurationJsonFiles(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            if (context.HostingEnvironment.IsProduction())
            {
                return;
            }

            var configurationDirectory = $"configuration{Path.PathSeparator}json";
            var directoryContents = context.HostingEnvironment.ContentRootFileProvider
                .GetDirectoryContents("configuration/json")
                .Where(fi => fi.Exists && !fi.IsDirectory && Path.GetExtension((string)fi.PhysicalPath).Contains("json"))
                .ToArray();

            foreach (IFileInfo fileInfo in directoryContents)
            {
                builder.AddJsonFile(fileInfo.PhysicalPath);
            }
        }
    }
}