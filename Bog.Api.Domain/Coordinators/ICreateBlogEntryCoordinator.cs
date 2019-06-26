using Bog.Api.Domain.Models;

namespace Bog.Api.Domain.Coordinators
{
    public interface ICreateBlogEntryCoordinator
    {
        void CreateNewEntry(NewEntryRequest newEntry);
    }
}