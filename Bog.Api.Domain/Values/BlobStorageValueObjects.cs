using System.Collections.Generic;

namespace Bog.Api.Domain.Values
{
    public static class BlobStorageValueObjects
    {
        public const string MARKDOWN_ARTICLE_ENTRIES_CONTAINER = "md-article-entries";
        public const string TRANSLATED_ARTICLE_ENTRIES_CONTAINER = "article-entries";
        public const string ENTRY_MEDIA_CONTAINER = "entry-media";
    }

    public enum BlobStorageContainer
    {
        MARKDOWN_ARTICLE_ENTRIES_CONTAINER,
        TRANSLATED_ARTICLE_ENTRIES_CONTAINER,
        ENTRY_MEDIA_CONTAINER
    }

    public static class BlobStorageLookupValueObjects
    {
        public static readonly IDictionary<BlobStorageContainer, string> BlobNameMap = new Dictionary<BlobStorageContainer, string>()
        {
            {BlobStorageContainer.MARKDOWN_ARTICLE_ENTRIES_CONTAINER, BlobStorageValueObjects.MARKDOWN_ARTICLE_ENTRIES_CONTAINER},
            {BlobStorageContainer.TRANSLATED_ARTICLE_ENTRIES_CONTAINER, BlobStorageValueObjects.TRANSLATED_ARTICLE_ENTRIES_CONTAINER},
            {BlobStorageContainer.ENTRY_MEDIA_CONTAINER, BlobStorageValueObjects.ENTRY_MEDIA_CONTAINER}

        };
    }
}