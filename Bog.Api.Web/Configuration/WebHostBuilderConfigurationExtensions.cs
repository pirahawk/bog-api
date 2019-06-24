﻿using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Bog.Api.Web.Configuration
{
    public static class WebHostBuilderConfigurationExtensions
    {
        public static IWebHostBuilder WithBlogApiConfigurationJson(this IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(AddConfigurationJsonFiles);
            return builder;
        }

        private static void AddConfigurationJsonFiles(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            var directoryContents = context.HostingEnvironment.ContentRootFileProvider
                .GetDirectoryContents("configuration")
                .Where(fi => fi.Exists && !fi.IsDirectory && Path.GetExtension((string) fi.PhysicalPath).Contains("json"))
                .ToArray();

            foreach (IFileInfo fileInfo in directoryContents)
            {
                builder.AddJsonFile(fileInfo.PhysicalPath);
            }
        }
    }
}