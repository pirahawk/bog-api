using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Coordinators
{
    public interface IEntryMediaSearchStrategy
    {
        Task<EntryMedia> Find(string mediaMD5Base64Hash);
    }
}