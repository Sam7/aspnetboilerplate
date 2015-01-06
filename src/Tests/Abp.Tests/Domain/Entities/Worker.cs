using Abp.Domain.Entities;

namespace Abp.Tests.Domain.Entities
{
    public class Worker : Entity<int>
    {
        public string Name { get; set; }
    }
}