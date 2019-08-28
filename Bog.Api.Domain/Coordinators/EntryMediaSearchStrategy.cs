using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.DbContext;

namespace Bog.Api.Domain.Coordinators
{
    public class EntryMediaSearchStrategy : IEntryMediaSearchStrategy
    {
        private readonly IBlogApiDbContext _context;

        public EntryMediaSearchStrategy(IBlogApiDbContext context)
        {
            _context = context;
        }

        public async Task<EntryMedia> Find(Guid entryId, string mediaMD5Base64Hash)
        {
            if (mediaMD5Base64Hash == null) throw new ArgumentNullException(nameof(mediaMD5Base64Hash));

            var entryMediae = _context.Query<EntryMedia>().ToArray();

            var result =  entryMediae
                .FirstOrDefault(em => em.EntryContentId == entryId && string.Equals(em.MD5Base64Hash, mediaMD5Base64Hash));

            return await Task.FromResult(result);
        }
    }
}