using System.Net.Http.Headers;

namespace Bog.Api.Common
{
    public static class HeaderUtilityHelper
    {
        public static string TryGetContentDispositionFileName(string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerValue))
            {
                return string.Empty;
            }

            if (!ContentDispositionHeaderValue.TryParse(headerValue, out ContentDispositionHeaderValue dispositionHeaderValue))
            {
                return null;
            }

            return dispositionHeaderValue.FileName.Replace("\"", string.Empty);
        }
    }
}