using System;
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

        public EntryMediaFixtrue()
        {
            Id = Guid.NewGuid();
            FileName = "foo.txt";
            BlobFileName = Guid.NewGuid();

            var entryContent = new EntryContentFixture().Build();
            EntryContentId = entryContent.Id;
            EntryContent = entryContent;
        }

        public EntryMedia Build()
        {
            return new EntryMedia
            {
                Id = Id,
                FileName = FileName,
                EntryContentId = EntryContentId,
                EntryContent = EntryContent,
                BlobFileName = BlobFileName
            };
        }
    }
}