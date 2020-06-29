using System;

namespace Bog.Api.Domain.Markdown
{
    public static class MarkdownConverterValuesObject
    {
        public static string GetConverterApiUrl(Guid articleId)
        {
            return $"convert/{articleId}";
        }
    }
}