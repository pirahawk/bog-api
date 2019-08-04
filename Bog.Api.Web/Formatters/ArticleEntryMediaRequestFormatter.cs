using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bog.Api.Common;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Bog.Api.Web.Formatters
{
    public class ArticleEntryMediaRequestFormatter : InputFormatter
    {
        public ArticleEntryMediaRequestFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaTypesValueObject.IMG));
        }

        public override bool CanRead(InputFormatterContext context)
        {
            return base.CanRead(context);
        }

        protected override bool CanReadType(Type type)
        {
            return type.IsAssignableFrom(typeof(ArticleEntryMediaRequest));
        }
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.ContentLength.HasValue || request.ContentLength == 0)
            {
                return await InputFormatterResult.FailureAsync();
            }

            var mediaRequest = new ArticleEntryMediaRequest();

            using (var streamContent = new StreamContent(request.Body))
            {
                var mediaBytes = await streamContent.ReadAsByteArrayAsync();
                var base64Hash = mediaBytes.ComputeMD5HashBase54();

                mediaRequest.MediaContent = mediaBytes;
                mediaRequest.ContentHashBase64 = base64Hash;
            }

            mediaRequest.FileName = TryGetFileName(context);

            return await InputFormatterResult.SuccessAsync(mediaRequest);
        }

        private string TryGetFileName(InputFormatterContext context)
        {
            var httpContextRequest = context.HttpContext.Request;

            if (!httpContextRequest.Headers.ContainsKey(HeaderNames.ContentDisposition))
            {
                return null;
            }

            var stringValues = httpContextRequest.Headers[HeaderNames.ContentDisposition];
            var contentDispositionString = stringValues.ToArray().FirstOrDefault();

            if(!ContentDispositionHeaderValue.TryParse(new StringSegment(contentDispositionString), out var dispositionHeaderValue))
            {
                return null;
            }

            return dispositionHeaderValue.FileName.Value;
        }
    }
}