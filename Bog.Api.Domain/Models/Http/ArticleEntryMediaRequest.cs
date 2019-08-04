using System;

namespace Bog.Api.Domain.Models.Http
{
    public class ArticleEntryMediaRequest
    {
        public Guid EntryId { get; set; }
        public byte[] MediaContent { get; set; }
        public string ContentHashBase64 { get; set; }
        public string FileName { get; set; }
    }
}