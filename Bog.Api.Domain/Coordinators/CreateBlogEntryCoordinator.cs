using System;
using System.Linq;
using Bog.Api.Domain.DbContext;
using Bog.Api.Domain.Models;

namespace Bog.Api.Domain.Coordinators
{
    public class CreateBlogEntryCoordinator : ICreateBlogEntryCoordinator
    {
        private readonly IBlogApiDbContext _context;

        public CreateBlogEntryCoordinator(IBlogApiDbContext context)
        {
            _context = context;
        }
        public void CreateNewEntry(NewEntryRequest newEntry)
        {
            if (newEntry == null) throw new ArgumentNullException(nameof(newEntry));

        }
    }
}