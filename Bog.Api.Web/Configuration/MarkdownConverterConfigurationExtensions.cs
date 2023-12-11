using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.Markdown;
using Bog.Api.Web.Markdown;

namespace Bog.Api.Web.Configuration
{
    public static class MarkdownConverterConfigurationExtensions
    {
        public static void WithMarkdownConverterHttpClient(this WebApplicationBuilder builder)
        {
            var configSection = builder.Configuration.GetSection("MarkdownConverterConfiguration");

            if (!configSection.Exists() || !configSection.GetChildren().Any())
            {
                throw new ApplicationException($"Could not bind to {typeof(MarkdownConverterConfiguration)} from applied configuration to create HTTP Client");
            }

            var markdownConfiguration = new MarkdownConverterConfiguration();
            configSection.Bind(markdownConfiguration);
            builder.Services.AddHttpClient(BogMarkdownConverter.HTTP_CLIENT_NAME, httpClient => {
                var uriBuilder = new UriBuilder()
                {
                    Scheme = markdownConfiguration.Scheme,
                    Host = markdownConfiguration.Host,
                };

                uriBuilder.Port = markdownConfiguration.Port.HasValue && markdownConfiguration.Port.Value > 0 ? markdownConfiguration.Port.Value : uriBuilder.Port;
                httpClient.BaseAddress = uriBuilder.Uri;
            });

            builder.Services.AddTransient<IBogMarkdownConverter, BogMarkdownConverter>();
        }
    }
}