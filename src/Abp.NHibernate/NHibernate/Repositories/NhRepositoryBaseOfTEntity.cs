using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.NHibernate.Repositories
{
    using System;

    /// <summary>
    /// A shortcut of <see cref="NhRepositoryBase{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class NhRepositoryBase<TEntity> : NhRepositoryBase<TEntity, Guid>, IRepository<TEntity> where TEntity : class, IEntity<Guid>
    {
        public NhRepositoryBase(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
        }
    }
}