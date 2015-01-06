namespace Abp.Domain.Entities
{
    using System;

    /// <summary>
    /// A shortcut of <see cref="IEntity{TPrimaryKey}"/> for most used primary key type (<see cref="Guid"/>).
    /// </summary>
    public interface IEntity : IEntity<Guid>
    {

    }
}