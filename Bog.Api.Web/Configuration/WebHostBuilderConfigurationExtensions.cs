using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Linq;
//using Microsoft.Azure.KeyVault;
//using Microsoft.Azure.Services.AppAuthentication;
//using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Azure.Security.KeyVault.Secrets;
using System;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Azure.Core;

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
            var keyVaultUrl = $"https://{keyVaultName}.vault.azure.net/";

            // TLDR I DON'T know if this works yet!! HAD to change this during the dotnet 6 upgrade. Check commit history

            //var client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());
            //var manager = new DefaultKeyVaultSecretManager();
            builder.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
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