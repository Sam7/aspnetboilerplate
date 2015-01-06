using Abp.Runtime.Session;

namespace Abp.Tests.Configuration
{
    using System;

    public class MyChangableSession : IAbpSession
    {
        public Guid? UserId { get; set; }

        public Guid? TenantId { get; set; }
    }
}