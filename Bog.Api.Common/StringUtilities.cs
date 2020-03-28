using System;
using System.Text;

namespace Bog.Api.Common
{
    public static class StringUtilities
    {
        public static string ToBase64(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var contentBytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(contentBytes);
        }

        public static string FromBase64(string base64EncodedString)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedString))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(base64EncodedString));

            var decodedBytes = Convert.FromBase64String(base64EncodedString);
            var decodedStr = Encoding.UTF8.GetString(decodedBytes);
            return decodedStr;
        }
    }
}