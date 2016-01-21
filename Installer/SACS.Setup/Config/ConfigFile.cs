using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using SACS.Setup.Classes;

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
        protected const string ConnectionStringCategory = "Default Connection String";

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
        /// Gets a value indicating whether there are unsaved changes to this config.
        /// </summary>
        /// <value>
        /// <c>true</c> if there are unsaved changes to this config; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool HasChanges { get; protected set; }

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
        /// Loads the config from the specified file.
        /// </summary>
        /// <param name="exeFile">The executable to open the configuration for.</param>
        /// <exception cref="System.ArgumentNullException">filename</exception>
        /// <exception cref="System.InvalidOperationException">SACS.WindowsService.exe.config is read-only or not found at location:  + configInfo.FullName</exception>
        public void RefreshFromFile(string exeFile)
        {
            this.UnderlyingConfig = ConfigurationManager.OpenExeConfiguration(exeFile);

            // TODO: Put any base property loads here, then call ReloadProperties()
            this.ReloadProperties();
        }

        /// <summary>
        /// Saves the current config changes to the underlying config file.
        /// </summary>
        public void SaveChanges()
        {
            FileSystemUtilities.BackupFile(this.UnderlyingConfig.FilePath);
            // TODO: Put any base config updates here.
            this.UpdateUnderlyingConfig();
            this.UnderlyingConfig.Save(ConfigurationSaveMode.Minimal, false);
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