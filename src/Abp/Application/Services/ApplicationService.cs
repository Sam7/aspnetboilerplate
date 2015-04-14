using System.Globalization;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using Castle.Core.Logging;

namespace Abp.Application.Services
{
    using System;

    /// <summary>
    /// This class can be used as a base class for application services. 
    /// </summary>
    public abstract class ApplicationService : IApplicationService
    {
        /// <summary>
        /// The localization manager.
        /// </summary>
        private ILocalizationManager localizationManager;

        /// <summary>
        /// The localization source name.
        /// </summary>
        private string localizationSourceName;

        /// <summary>
        /// Gets current session information.
        /// </summary>
        public IAbpSession CurrentSession { protected get; set; }

        /// <summary>
        /// Reference to the permission manager.
        /// </summary>
        public IPermissionManager PermissionManager { protected get; set; }

        /// <summary>
        /// Reference to the setting manager.
        /// </summary>
        public ISettingManager SettingManager { protected get; set; }

        /// <summary>
        /// Reference to the localization manager.
        /// </summary>
        public ILocalizationManager LocalizationManager {
            protected get
            {
                return this.localizationManager;
            }

            set
            {
                if (this.localizationManager == value)
                    return;
                this.localizationManager = value;
                if (this.localizationSourceName != null)
                    this.LocalizationSource = this.LocalizationManager.GetSource(this.localizationSourceName);
            }
        }

        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// </summary>
        protected string LocalizationSourceName
        {
            get
            {
                return this.LocalizationSource.Name;
            }

            set
            {
                if (this.localizationSourceName != null && this.localizationSourceName.Equals(value, StringComparison.InvariantCulture))
                    return;
                this.localizationSourceName = value;
                if (this.localizationSourceName != null)
                    this.LocalizationSource = this.LocalizationManager.GetSource(this.localizationSourceName);
            }
        }

        /// <summary>
        /// Gets localization source.
        /// It's valid if <see cref="LocalizationSourceName"/> is set.
        /// </summary>
        protected ILocalizationSource LocalizationSource { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ApplicationService()
        {
            CurrentSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
            LocalizationSource = NullLocalizationSource.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }
    }
}
