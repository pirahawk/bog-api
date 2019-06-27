using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bog.Api.Db.DbContexts
{
    public partial class BlogApiDbContext
    {
        private IDictionary<Type, IEntityType> MappedTypesLookup;

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            if (MappedTypesLookup == null)
            {
                InitializeMappedTypesLookup();
            }

            if (!MappedTypesLookup.ContainsKey(typeof(TEntity)))
            {
                throw new ArgumentException($"There is no mapped entity of type: {typeof(TEntity)} in the DBContext");
            }

            return Set<TEntity>().AsQueryable();
        }

        private void InitializeMappedTypesLookup()
        {
            this.MappedTypesLookup = Model.GetEntityTypes().ToDictionary(et => et.DefiningEntityType.ClrType, et => et);
        }
    }
}