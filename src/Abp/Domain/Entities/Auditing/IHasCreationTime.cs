using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if <see cref="CreationTimeUtc"/> of this entity must be stored.
    /// <see cref="CreationTimeUtc"/> can be automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreationTimeUtc { get; set; }
    }
}