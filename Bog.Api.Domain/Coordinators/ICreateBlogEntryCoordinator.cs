using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models;

namespace Bog.Api.Domain.Coordinators
{
    public interface ICreateBlogEntryCoordinator
    {
        Task<Article> CreateNewEntryAsync(NewEntryRequest request);
    }
}