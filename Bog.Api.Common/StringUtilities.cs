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
    }
}