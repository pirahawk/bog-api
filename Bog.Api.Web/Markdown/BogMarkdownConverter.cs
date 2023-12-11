using Bog.Api.Common;
using Bog.Api.Domain.Markdown;
using Bog.Api.Domain.Models.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace Bog.Api.Web.Markdown
{
    public class BogMarkdownConverter : IBogMarkdownConverter
    {
        public const string HTTP_CLIENT_NAME = "bogMarkdownConverterClient";
        private readonly IHttpClientFactory clientFactory;
        private JsonSerializerOptions _jsonOptions
        {
            get
            {
                return new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IgnoreNullValues = true,
                    AllowTrailingCommas = true,
                    WriteIndented = true
                };
            }
        }

        public BogMarkdownConverter(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<string> ConvertArticle(Guid articleId, string mdContentUrl)
        {
            if (string.IsNullOrWhiteSpace(mdContentUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mdContentUrl));

            var apiRoute = MarkdownConverterValuesObject.GetConverterApiUrl(articleId);
            var requestModel = new MarkdownConverterRequest
            {
                Content = StringUtilities.ToBase64(mdContentUrl)
            };
            var serializedRequestModel = JsonSerializer.Serialize(requestModel, _jsonOptions);
            var request = new HttpRequestMessage(HttpMethod.Post, apiRoute);
            request.Content = new StringContent(serializedRequestModel);
            request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json);

            var response = await clientFactory.CreateClient(HTTP_CLIENT_NAME).SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not convert Markdown content for Article: {articleId} at location {mdContentUrl}");
            }

            var convertedContent = await response.Content.ReadAsStringAsync();
            return convertedContent;
        }
    }
}