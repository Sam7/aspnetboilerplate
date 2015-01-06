using Abp.Domain.Entities;

namespace Abp.NHibernate.EntityMappings
{
    using System;

    /// <summary>
    /// A shortcut of <see cref="EntityMap{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="Guid"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity map</typeparam>
    public abstract class EntityMap<TEntity> : EntityMap<TEntity, Guid> where TEntity : IEntity<Guid>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tableName">Table name</param>
        protected EntityMap(string tableName)
            : base(tableName)
        {

        }
    }
}