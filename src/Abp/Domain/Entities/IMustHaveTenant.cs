﻿namespace Abp.Domain.Entities
{
    using System;

    /// <summary>
    /// Implement this interface for an entity which must have TenantId.
    /// </summary>
    public interface IMustHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        Guid tenantId { get; set; }
    }
}