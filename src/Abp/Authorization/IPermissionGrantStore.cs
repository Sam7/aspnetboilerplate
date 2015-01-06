namespace Abp.Authorization
{
    using System;

    /// <summary>
    /// This class is used to perform persistent operations for permissions.
    /// </summary>
    public interface IPermissionGrantStore
    {
        /// <summary>
        /// Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="userId">Id of the user to check</param>
        /// <param name="permissionName">Name of the permission</param>
        bool IsGranted(Guid userId, string permissionName);
    }
}