using System;

namespace Bog.Api.Domain.Models.Http
{
    public class ArticleEntryMediaRequest
    {
        public Guid EntryId { get; set; }
        public byte[] MediaContent { get; set; }
        public string MD5Base64Hash { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}