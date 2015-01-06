namespace Abp.Domain.Entities
{
    using System;

    /// <summary>
    /// Implement this interface for an entity which may optionally have TenantId.
    /// </summary>
    public interface IMayHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        Guid? tenantId { get; set; }
    }
}