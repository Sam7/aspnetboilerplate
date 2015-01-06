namespace Abp.Domain.Entities.Auditing
{
    using System;

    /// <summary>
    /// A shortcut of <see cref="CreationAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="Guid"/>).
    /// </summary>
    public abstract class CreationAuditedEntity : CreationAuditedEntity<Guid>
    {

    }
}