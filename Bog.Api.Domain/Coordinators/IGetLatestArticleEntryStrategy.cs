using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;

namespace Bog.Api.Domain.Coordinators
{
    public interface IGetLatestArticleEntryStrategy
    {
        Task<EntryContent> FindLatestEntry(Guid articleId);
    }
}