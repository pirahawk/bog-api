using Microsoft.Extensions.Logging;

namespace Bog.Api.Domain.Values
{
    public static class LogEvenIdsValueObject
    {
        public static EventId EnitityFramework = new EventId(1, "entity-framework");
        public static EventId BlobStorage = new EventId(2, "azure-blob-storage");

    }
}