using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bog.Api.Web.Formatters
{
    public class EntryContentFormatter : TextInputFormatter
    {
        public EntryContentFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypesValueObject.TEXT_PLAIN));
        }

        protected override object GetDefaultValueForType(Type modelType)
        {
            return CheckType(modelType)? null : base.GetDefaultValueForType(modelType);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return CanReadType(context.ModelType);
        }

        protected override bool CanReadType(Type type)
        {
            return CheckType(type);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            return await ReadRequestBodyAsync(context);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            return await ReadAsync(context);
        }

        public override async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var httpContextRequest = context.HttpContext.Request;

            if (!httpContextRequest.ContentLength.HasValue || httpContextRequest.ContentLength == 0)
            {
                return await InputFormatterResult.FailureAsync();
            }

            using (var reader = context.ReaderFactory(httpContextRequest.Body, Encoding.UTF8))
            {
                var blogPost = new ArticleEntry();
                string readToEndAsync = await reader.ReadToEndAsync();
                blogPost.Content = readToEndAsync;
                return await InputFormatterResult.SuccessAsync(blogPost);
            }
        }

        private static bool CheckType(Type type)
        {
            return typeof(ArticleEntry).IsAssignableFrom(type);
        }

    }
}