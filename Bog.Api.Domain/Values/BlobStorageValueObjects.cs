namespace Bog.Api.Domain.Values
{
    public static class BlobStorageValueObjects
    {
        public const string MARKDOWN_ARTICLE_ENTRIES_CONTAINER = "md-article-entries";
        public const string TRANSLATED_ARTICLE_ENTRIES_CONTAINER = "article-entries";
    }

    public enum BlobStorageContainer
    {
        MARKDOWN_ARTICLE_ENTRIES_CONTAINER,
        TRANSLATED_ARTICLE_ENTRIES_CONTAINER
    }
}