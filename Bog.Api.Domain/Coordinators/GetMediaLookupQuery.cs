using System;
using System.Collections.Generic;
using System.Linq;
using Bog.Api.Common;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class GetMediaLookupQuery : IGetMediaLookupQuery
    {
        private IBlogApiDbContext _dbContext;

        public GetMediaLookupQuery(IBlogApiDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Dictionary<string, string> CreateMediaLookup(Guid articleId)
        {
            var articlesToQuery = _dbContext.Query<Article>().Where(article => article.Id == articleId);

            var query = from article in _dbContext.Query<Article>().Where(article => article.Id == articleId)

                let allPersistedArticleMedia = article.ArticleEntries
                    .SelectMany(articleEntry => articleEntry.EntryMedia)
                    .Where(entryMedia => entryMedia.Persisted.HasValue)

                let mediaProjection = allPersistedArticleMedia.Select(media => new
                {
                    media.FileName,
                    TopDownloadLink = allPersistedArticleMedia
                        .Where(em => em.FileName.Equals(media.FileName))
                        .OrderByDescending(em => em.Persisted)
                        .Select(em => em.BlobUrl)
                        .FirstOrDefault()
                })

                select mediaProjection;

            var allMediaProjection = query
                .SelectMany(mediaProjection => mediaProjection)
                .Select(mediaProjection => new MediaProjection
                {
                    FileName = mediaProjection.FileName,
                    DownloadLink = mediaProjection.TopDownloadLink
                })
                .ToArray()
                .Distinct(MediaProjection.FileNameComparer)
                .ToDictionary(lookup => lookup.FileName, lookup => lookup.DownloadLink);

            return allMediaProjection;
        }

        sealed class MediaProjection
        {
            public string FileName { get; set; }
            public string DownloadLink { get; set; }

            sealed class FileNameEqualityComparer : IEqualityComparer<MediaProjection>
            {
                public bool Equals(MediaProjection x, MediaProjection y)
                {
                    if (ReferenceEquals(x, y)) return true;
                    if (ReferenceEquals(x, null)) return false;
                    if (ReferenceEquals(y, null)) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return x.FileName == y.FileName;
                }

                public int GetHashCode(MediaProjection obj)
                {
                    return (obj.FileName != null ? obj.FileName.GetHashCode() : 0);
                }
            }

            public static IEqualityComparer<MediaProjection> FileNameComparer { get; } = new FileNameEqualityComparer();
        }
    }
}