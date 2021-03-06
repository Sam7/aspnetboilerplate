﻿using System.Collections.Generic;

namespace Abp.Application.Navigation
{
    using System;

    /// <summary>
    /// Used to manage navigation for users.
    /// </summary>
    public interface IUserNavigationManager
    {
        /// <summary>
        /// Gets a menu specialized for given user.
        /// </summary>
        /// <param name="menuName">Unique name of the menu</param>
        /// <param name="userId">User id or null for anonymous users</param>
        UserMenu GetMenu(string menuName, Guid? userId);

        /// <summary>
        /// Gets all menus specialized for given user.
        /// </summary>
        /// <param name="userId">User id or null for anonymous users</param>
        IReadOnlyList<UserMenu> GetMenus(Guid? userId);
    }
}