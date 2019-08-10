using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public interface ICreateArticleEntryCoordinator
    {
        Task<EntryContent> CreateArticleEntry(Guid articleId, ArticleEntry entry);
        Task<EntryContent> MarkUploadSuccess(EntryContent entryContent, string uploadUrl);
    }
}