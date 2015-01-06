namespace Abp.Runtime.Session
{
    using System;

    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// </summary>
    public interface IAbpSession
    {
        /// <summary>
        /// Gets current UserId of null.
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// Gets current TenantId or null.
        /// </summary>
        Guid? TenantId { get; }
    }
}
