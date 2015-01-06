using System.Data.Entity;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    using System;

    public class EfRepositoryBase<TDbContext, TEntity> : EfRepositoryBase<TDbContext, TEntity, Guid>, IRepository<TEntity>
        where TEntity : class, IEntity<Guid>
        where TDbContext : DbContext
    {
        public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}