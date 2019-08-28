using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Coordinators
{
    public interface IEntryMediaSearchStrategy
    {
        Task<EntryMedia> Find(Guid entryId, string mediaMD5Base64Hash);
    }
}