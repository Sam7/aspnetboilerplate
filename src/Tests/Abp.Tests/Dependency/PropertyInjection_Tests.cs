using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Abp.Tests.Dependency
{
    public class PropertyInjection_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void Should_Inject_Session_For_ApplicationService()
        {
            var session = Substitute.For<IAbpSession>();
            session.TenantId.Returns(Guid.NewGuid());
            session.UserId.Returns(Guid.NewGuid());

            LocalIocManager.Register<MyApplicationService>();
            LocalIocManager.IocContainer.Register(
                Component.For<IAbpSession>().UsingFactoryMethod(() => session)
                );

            var myAppService = LocalIocManager.Resolve<MyApplicationService>();
            myAppService.TestSession();
        }

        private class MyApplicationService : ApplicationService
        {
            public void TestSession()
            {
                CurrentSession.ShouldNotBe(null);
                CurrentSession.TenantId.ShouldBe(Guid.NewGuid());
                CurrentSession.UserId.ShouldBe(Guid.NewGuid());
            }
        }
    }
}
