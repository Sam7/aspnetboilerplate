namespace Abp.Domain.Entities.Auditing
{
    using System;

    /// <summary>
    /// A shortcut of <see cref="AuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public abstract class AuditedEntity : AuditedEntity<Guid>
    {

    }
}