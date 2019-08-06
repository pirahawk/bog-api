using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public interface IUploadArticleEntryCoordinator
    {
        Task<string> UploadArticleEntry(EntryContent entryContent, ArticleEntry articleEntry);
    }
}