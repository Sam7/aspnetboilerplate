namespace Abp.Runtime.Session
{
    using System;

    /// <summary>
    /// Implements null object pattern for <see cref="IAbpSession"/>.
    /// </summary>
    public class NullAbpSession : IAbpSession
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullAbpSession Instance { get { return SingletonInstance; } }
        private static readonly NullAbpSession SingletonInstance = new NullAbpSession();

        /// <inheritdoc/>
        public Guid? UserId { get { return null; } }

        /// <inheritdoc/>
        public Guid? TenantId { get { return null; } }

        private NullAbpSession()
        {

        }
    }
}