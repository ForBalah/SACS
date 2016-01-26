using System.ComponentModel;
using System.Configuration;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Web.UI.Design;
using SACS.Setup.Classes;
using SACS.Setup.Log;

namespace SACS.Setup.Config
{
    /// <summary>
    /// The base class that represents a config file.
    /// </summary>
    public abstract class ConfigFile
    {
        #region Fields

        /// <summary>
        /// The connection string category
        /// </summary>
        protected const string ConnectionStringCategory = "Database Connection Settings";

        /// <summary>
        /// The application settings category
        /// </summary>
        protected const string AppSettingsCategory = "AppSettings";

        /// <summary>
        /// The mail settings category
        /// </summary>
        protected const string MailSettingsCategory = "Mail Settings";

        private static LogHelper _logger = LogHelper.GetLogger(typeof(ConfigFile));

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFile" /> class.
        /// </summary>
        protected ConfigFile()
        {
            this.HasChanges = false;
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets or sets the default size of the paging.
        /// </summary>
        /// <value>
        /// The default size of the paging.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Default paging size")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(50)]
        [Description("The value used for paging throughout the system. Applies primarily to logs. A size of '0' indicates unbounded page size.")]
        public int DefaultPagingSize { get; set; }

        /// <summary>
        /// Gets a value indicating whether there are unsaved changes to this config.
        /// </summary>
        /// <value>
        /// <c>true</c> if there are unsaved changes to this config; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool HasChanges { get; protected set; }

        /// <summary>
        /// Gets or sets the server address.
        /// </summary>
        /// <value>
        /// The server address.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Server Address")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue("http://localhost:3800/")]
        [Description("The base URL (with port) that the server is accessible through. Default is http://localhost:3800/ for the server and http://localhost:3800/api for the management consoles.")]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        public string ServerAddress { get; set; }

        /// <summary>
        /// Gets the underlying configuration.
        /// </summary>
        /// <value>
        /// The underlying configuration.
        /// </value>
        protected Configuration UnderlyingConfig { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the application setting value returning an empty string if not found.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetAppSettingValue(string key)
        {
            return (this.UnderlyingConfig.AppSettings.Settings[key] ?? new KeyValueConfigurationElement(key, string.Empty)).Value;
        }

        /// <summary>
        /// Loads the config from the specified file.
        /// </summary>
        /// <param name="exeFile">The executable to open the configuration for.</param>
        /// <exception cref="System.ArgumentNullException">filename</exception>
        /// <exception cref="System.InvalidOperationException">SACS.WindowsService.exe.config is read-only or not found at location:  + configInfo.FullName</exception>
        public void RefreshFromFile(string exeFile)
        {
            this.UnderlyingConfig = ConfigurationManager.OpenExeConfiguration(exeFile);

            if (!this.UnderlyingConfig.HasFile)
            {
                throw new FileNotFoundException(string.Format("The config file for {0} cannot be found. Try reinstalling SACS.", exeFile));
            }

            this.DefaultPagingSize = int.Parse("0" + this.GetAppSettingValue("System.DefaultPagingSize"));
            this.ServerAddress = this.GetAppSettingValue("WebAPI.BaseAddress");
            this.ReloadProperties();
        }

        /// <summary>
        /// Saves the current config changes to the underlying config file.
        /// </summary>
        public void SaveChanges()
        {
            FileSystemUtilities.BackupFile(this.UnderlyingConfig.FilePath);

            this.SetAppSettingValue("System.DefaultPagingSize", this.DefaultPagingSize.ToString());
            this.SetAppSettingValue("WebAPI.BaseAddress", this.ServerAddress);
            this.UpdateUnderlyingConfig();

            _logger.Log("Saving config changes to " + this.UnderlyingConfig.FilePath);
            this.UnderlyingConfig.Save(ConfigurationSaveMode.Minimal, false);
        }

        /// <summary>
        /// Sets the application setting value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetAppSettingValue(string key, string value)
        {
            if (this.UnderlyingConfig.AppSettings.Settings.AllKeys.Contains(key))
            {
                this.UnderlyingConfig.AppSettings.Settings[key].Value = value;
            }
            else
            {
                this.UnderlyingConfig.AppSettings.Settings.Add(new KeyValueConfigurationElement(key, value));
            }
        }

        /// <summary>
        /// Reloads the properties from the configXml
        /// </summary>
        protected abstract void ReloadProperties();

        /// <summary>
        /// Updates the underlying configuration.
        /// </summary>
        protected abstract void UpdateUnderlyingConfig();

        #endregion Methods
    }
}