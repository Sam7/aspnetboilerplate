using Abp.Domain.Entities;

namespace Abp.Tests.Domain.Entities
{
    public class Department : Entity<int>
    {
        public string Name { get; set; }
    }
}