﻿using System.Linq;
using Abp.Application.Navigation;
using Shouldly;
using Xunit;

namespace Abp.Tests.Application.Navigation
{
    using System;

    public class Menu_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public void Test_Menu_System()
        {
            var testCase = new NavigationTestCase();

            //Check created menu definitions
            var mainMenuDefinition = testCase.NavigationManager.MainMenu;
            mainMenuDefinition.Items.Count.ShouldBe(1);

            var adminMenuItemDefinition = mainMenuDefinition.GetItemByNameOrNull("Abp.Zero.Administration");
            adminMenuItemDefinition.ShouldNotBe(null);
            adminMenuItemDefinition.Items.Count.ShouldBe(3);
            
            //Check user menus
            var userId = Guid.NewGuid();
            var userMenu = testCase.UserNavigationManager.GetMenu(mainMenuDefinition.Name, userId);
            userMenu.Items.Count.ShouldBe(1);

            var userAdminMenu = userMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration");
            userAdminMenu.ShouldNotBe(null);

            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.User").ShouldNotBe(null);
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.Role").ShouldBe(null);
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.Setting").ShouldNotBe(null);
        }
    }
}
