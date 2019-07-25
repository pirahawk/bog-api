using System;

namespace Bog.Api.Domain.Data
{
    public class EntryMedia
    {
        public Guid Id { get; set; }
        public Guid EntryContentId { get; set; }
        public EntryContent EntryContent { get; set; }
        public string FileName { get; set; }
        public Guid BlobFileName { get; set; }
    }
}