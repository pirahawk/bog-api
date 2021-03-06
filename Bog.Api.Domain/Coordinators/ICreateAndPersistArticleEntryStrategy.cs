﻿using System;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Models.Http;

namespace Bog.Api.Domain.Coordinators
{
    public interface ICreateAndPersistArticleEntryStrategy
    {
        Task<EntryContent> PersistArticleEntryAsync(Guid articleId, ArticleEntry entry);
    }
}