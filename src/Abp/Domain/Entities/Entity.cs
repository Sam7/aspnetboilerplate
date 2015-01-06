namespace Abp.Domain.Entities
{
    using System;

    /// <summary>
    /// A shortcut of <see cref="Entity{TPrimaryKey}"/> for most used primary key type (<see cref="Guid"/>).
    /// </summary>
    public abstract class Entity : Entity<Guid>, IEntity
    {

    }
}