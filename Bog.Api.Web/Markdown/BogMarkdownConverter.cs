using Bog.Api.Common;
using Bog.Api.Domain.Markdown;
using Bog.Api.Domain.Models.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bog.Api.Web.Markdown
{
    public class BogMarkdownConverter : IBogMarkdownConverter
    {
        private readonly HttpClient _bogHttpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public BogMarkdownConverter(HttpClient bogHttpClient, JsonSerializerOptions jsonOptions)
        {
            _bogHttpClient = bogHttpClient;
            _jsonOptions = jsonOptions;
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

            var response = await _bogHttpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not convert Markdown content for Article: {articleId} at location {mdContentUrl}");
            }

            var convertedContent = await response.Content.ReadAsStringAsync();
            return convertedContent;
        }
    }
}