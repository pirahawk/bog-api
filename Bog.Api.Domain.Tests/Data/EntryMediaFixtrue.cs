using System;
using System.Net.Mime;
using Bog.Api.Common.Tests.Time;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Tests.Data
{
    public class EntryMediaFixtrue
    {
        public EntryContent EntryContent { get; set; }

        public Guid BlobFileName { get; set; }

        public Guid EntryContentId { get; set; }

        public string FileName { get; set; }

        public Guid Id { get; set; }

        public string MD5Base64Hash { get; set; }

        public DateTimeOffset? Persisted { get; set; }

        public string BlobUrl { get; set; }

        public DateTimeOffset Created { get; set; }

        public string ContentType { get; set; }

        public EntryMediaFixtrue()
        {
            Id = Guid.NewGuid();
            FileName = "foo.txt";
            BlobFileName = Guid.NewGuid();

            var entryContent = new EntryContentFixture().Build();
            EntryContentId = entryContent.Id;
            EntryContent = entryContent;
            MD5Base64Hash = "someHash";
            Persisted = new MockClock().Now;
            BlobUrl = "someUrl";
            Created = new MockClock().Now;
            ContentType = "someContentType";
        }

        public EntryMedia Build()
        {
            return new EntryMedia
            {
                Id = Id,
                FileName = FileName,
                EntryContentId = EntryContentId,
                EntryContent = EntryContent,
                BlobFileName = BlobFileName,
                ContentType = ContentType,
                Created = Created,
                BlobUrl = BlobUrl,
                Persisted = Persisted,
                MD5Base64Hash = MD5Base64Hash,
            };
        }

        
    }
}