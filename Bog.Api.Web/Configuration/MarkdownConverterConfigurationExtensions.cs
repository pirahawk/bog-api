using System;
using System.Net.Http;
using System.Text.Json;
using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.Markdown;
using Bog.Api.Web.Markdown;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bog.Api.Web.Configuration
{
    public static class MarkdownConverterConfigurationExtensions
    {
        public static void WithMarkdownConverterClient(this IServiceCollection services)
        {
            services.AddTransient<IBogMarkdownConverter>(sp =>
            {
                var markdownConfig = sp.GetService<IOptionsMonitor<MarkdownConverterConfiguration>>();
                if (markdownConfig.CurrentValue == null)
                {
                    throw new ArgumentException($"Could not find MarkdownConverterConfiguration to create HTTP Client");
                }

                var uriBuilder = new UriBuilder()
                {
                    Scheme = markdownConfig.CurrentValue.Scheme,
                    Host = markdownConfig.CurrentValue.Host,
                };

                if (int.TryParse(markdownConfig.CurrentValue.Port, out var port))
                {
                    uriBuilder.Port = port;
                }

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IgnoreNullValues = true,
                    AllowTrailingCommas = true,
                    WriteIndented = true
                };

                var httpClient = new HttpClient();
                httpClient.BaseAddress = uriBuilder.Uri;
                var converter = new BogMarkdownConverter(httpClient, jsonOptions);
                return converter;
            });
        }
    }
}