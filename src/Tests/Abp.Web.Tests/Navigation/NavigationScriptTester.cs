using Abp.Tests.Application.Navigation;
using Abp.Tests.Configuration;
using Abp.Web.Navigation;
using Shouldly;
using Xunit;

namespace Abp.Web.Tests.Navigation
{
    using System;

    public class NavigationScript_Tests
    {
        [Fact]
        public void Test1()
        {
            var testCase = new NavigationTestCase();
            var scriptManager = new NavigationScriptManager(testCase.UserNavigationManager)
            {
                AbpSession = new MyChangableSession { UserId = Guid.NewGuid() }
            };

            var script = scriptManager.GetScript();
            script.ShouldNotBeNullOrEmpty();
        }
    }
}
