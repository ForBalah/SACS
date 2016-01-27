using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace SACS.Setup.Config
{
    /// <summary>
    /// The representation of the Windows management console app.config file.
    /// </summary>
    public class WindowsConsoleConfigFile : ConfigFile
    {
        #region Properties

        [Category(AppSettingsCategory)]
        [DisplayName("Chart Look-back Days")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(2)]
        [Description("The number of days to fetch performance chart data for. Default is '2'.")]
        public int ChartLookBackDays { get; set; }

        [Category(AppSettingsCategory)]
        [DisplayName("Server Connection Timeout")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(15)]
        [Description("The number of minutes that individual web API requests to the server will timeout.")]
        public int WebApiTimeoutSeconds { get; set; }

        [Category(AppSettingsCategory)]
        [DisplayName("Alternative Log Location")]
        [DefaultValue("..\\..\\..\\..\\services\\SACS.WindowsService\\bin\\Debug\\Logs")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("If the server goes down, this is where the logs can also be searched for physically.")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string AlternativeLogLocation { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Reloads the properties from the configXml
        /// </summary>
        protected override void ReloadProperties()
        {
            this.ChartLookBackDays = int.Parse("0" + this.GetAppSettingValue("Performance.LookBackDays"));
            this.WebApiTimeoutSeconds = int.Parse("0" + this.GetAppSettingValue("WebAPI.Timeout"));
            this.AlternativeLogLocation = this.GetAppSettingValue("Logs.AlternateLocation");
        }

        /// <summary>
        /// Updates the underlying configuration.
        /// </summary>
        protected override void UpdateUnderlyingConfig()
        {
            this.SetAppSettingValue("Performance.LookBackDays", this.ChartLookBackDays.ToString());
            this.SetAppSettingValue("WebAPI.Timeout", this.WebApiTimeoutSeconds.ToString());
            this.SetAppSettingValue("Logs.AlternateLocation", this.AlternativeLogLocation);
        }

        #endregion Methods
    }
}