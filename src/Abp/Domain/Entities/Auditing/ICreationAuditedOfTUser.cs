namespace Abp.Domain.Entities.Auditing
{
    using System;

    /// <summary>
    /// Adds navigation properties to <see cref="ICreationAudited"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    public interface ICreationAudited<TUser> : ICreationAudited
        where TUser : IEntity<Guid>
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        TUser CreatorUser { get; set; }
    }
}