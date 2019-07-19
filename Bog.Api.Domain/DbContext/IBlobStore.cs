using System.Threading.Tasks;
using Bog.Api.Domain.Values;

namespace Bog.Api.Domain.DbContext
{
    public interface IBlobStore
    {
        Task<bool> TryCreateContainer(BlobStorageContainer container);
    }
}