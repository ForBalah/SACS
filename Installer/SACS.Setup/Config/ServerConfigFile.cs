using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Globalization;
using System.Net.Configuration;
using System.Net.Mail;
using System.Windows.Forms.Design;

namespace SACS.Setup.Config
{
    /// <summary>
    /// The representation of the server app.config file.
    /// </summary>
    public class ServerConfigFile : ConfigFile
    {
        #region Properties

        /// <summary>
        /// Gets the SQL data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [Browsable(false)]
        public string DatabaseLocation
        {
            get
            {
                var entityPart = new EntityConnectionStringBuilder(this.ConnectionString.Settings.ConnectionString);
                var sqlPart = new SqlConnectionStringBuilder(entityPart.ProviderConnectionString);
                return string.Format("{0}.{1}", sqlPart.DataSource, string.IsNullOrWhiteSpace(sqlPart.InitialCatalog) ? sqlPart.AttachDBFilename : sqlPart.InitialCatalog);
            }
        }

        /// <summary>
        /// Gets or sets the default connection string.
        /// </summary>
        /// <value>
        /// The default connection string.
        /// </value>
        [Category(ConnectionStringCategory)]
        [DisplayName("Connection String")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The connection string used to connect to the datasource. Default is \"(LocalDB)\\v11.0\"")]
        public ConnectionStringBuilderFacade ConnectionString { get; private set; }

        /// <summary>
        /// Gets or sets the graph maximum points.
        /// </summary>
        /// <value>
        /// The graph maximum points.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Performance - Graph Max Points")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(50)]
        [Description("The average upper bound of points to show in the performance graphs.")]
        public int GraphMaxPoints { get; set; }

        /// <summary>
        /// Gets or sets the graph threshold.
        /// </summary>
        /// <value>
        /// The graph threshold.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Performance - Graph Threshold")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(0.15)]
        [Description("Percentage value, between 0 and 1, indicating how different the value of points should be before plotting on the graph.")]
        public decimal GraphThreshold { get; set; }

        /// <summary>
        /// Gets the mail settings.
        /// </summary>
        /// <value>
        /// The mail settings.
        /// </value>
        [Browsable(false)]
        public MailSettingsSectionGroup MailSettings { get; private set; }

        /// <summary>
        /// Gets or sets the SMTP delivery method.
        /// </summary>
        /// <value>
        /// The delivery method.
        /// </value>
        [Category(MailSettingsCategory)]
        [DisplayName("Delivery Method")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(SmtpDeliveryMethod.Network)]
        [Description("The delivery method for outgoing mails. The default is Network.")]
        public SmtpDeliveryMethod SmtpDeliveryMethod
        {
            get
            {
                return this.MailSettings.Smtp.DeliveryMethod;
            }

            set
            {
                this.MailSettings.Smtp.DeliveryMethod = value;

                // I want to refresh the SmtpPickupDirectoryLocation and NetworkHost, but as it stands
                // I'm not sure how to do that without accessing the set of the properties
                this.UpdatePickupDirectoryLocation(this.SmtpPickupDirectoryLocation);
                this.UpdateNetworkHost(this.SmtpNetworkHost);
                this.UpdateNetworkUserName(this.SmtpNetworkUserName);
                this.UpdateNetworkPassword(this.SmtpNetworkPassword);
                this.UpdateNetworkPort(this.SmtpNetworkPort);
            }
        }

        /// <summary>
        /// Gets or sets the SMTP mail from.
        /// </summary>
        /// <value>
        /// The SMTP mail from.
        /// </value>
        [Category(MailSettingsCategory)]
        [DisplayName("From")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The default email address whom the emails will be appear from.")]
        public string SmtpMailFrom
        {
            get
            {
                return this.MailSettings.Smtp.From;
            }

            set
            {
                this.MailSettings.Smtp.From = value;
            }
        }

        /// <summary>
        /// Gets or sets the SMTP pickup directory location.
        /// </summary>
        /// <value>
        /// The SMTP pickup directory location.
        /// </value>
        [Category(MailSettingsCategory)]
        [DisplayName("Pickup Directory Location")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The folder where SACS will save email messages to be processed by an SMTP server (if Delivery method is 'SpecifiedPickupDirectory').")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string SmtpPickupDirectoryLocation
        {
            get
            {
                return this.MailSettings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation;
            }

            set
            {
                this.UpdatePickupDirectoryLocation(value);
            }
        }

        /// <summary>
        /// Gets or sets the SMTP network host.
        /// </summary>
        /// <value>
        /// The SMTP network host.
        /// </value>
        [Category(MailSettingsCategory)]
        [DisplayName("Mail Network Host")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The name of the SMTP server.")]
        public string SmtpNetworkHost
        {
            get
            {
                return this.MailSettings.Smtp.Network.Host;
            }

            set
            {
                this.UpdateNetworkHost(value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the SMTP user.
        /// </summary>
        /// <value>
        /// The name of the SMTP user.
        /// </value>
        [Category(MailSettingsCategory)]
        [DisplayName("Mail Network User Name")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The user name to connect to an SMTP mail server.")]
        public string SmtpNetworkUserName
        {
            get
            {
                return this.MailSettings.Smtp.Network.UserName;
            }

            set
            {
                this.UpdateNetworkUserName(value);
            }
        }

        /// <summary>
        /// Gets or sets the SMTP password.
        /// </summary>
        /// <value>
        /// The SMTP password.
        /// </value>
        [Category(MailSettingsCategory)]
        [DisplayName("Mail Network Password")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The password to use to connect to an SMTP mail server.")]
        public string SmtpNetworkPassword
        {
            get
            {
                return this.MailSettings.Smtp.Network.Password;
            }

            set
            {
                this.UpdateNetworkPassword(value);
            }
        }

        [Category(MailSettingsCategory)]
        [DisplayName("Mail Network Port")]
        [DefaultValue(25)]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The port that SMTP clients use to connect to the server. The default is 25.")]
        public int SmtpNetworkPort
        {
            get
            {
                return this.MailSettings.Smtp.Network.Port;
            }

            set
            {
                this.UpdateNetworkPort(value);
            }
        }

        /// <summary>
        /// Gets or sets the support email address.
        /// </summary>
        /// <value>
        /// The support email address.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Support Email")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The email address to send any service failures to.")]
        public string SupportEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the success email template path.
        /// </summary>
        /// <value>
        /// The success email template path.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Success Email Template Path")]
        [DefaultValue("Templates/Email/SuccessEmailTemplate.html")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The path to the email template that will be used to generate service success emails with. The default is 'Templates/Email/SuccessEmailTemplate.html'")]
        public string SuccessEmailTemplatePath { get; set; }

        /// <summary>
        /// Gets or sets the support email template path.
        /// </summary>
        /// <value>
        /// The support email template path.
        /// </value>
        [Category(AppSettingsCategory)]
        [DisplayName("Support Email Template Path")]
        [DefaultValue("Templates/Email/SupportEmailTemplate.html")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("The path to the email template that will be used to generate service failure emails with. The default is 'Templates/Email/SupportEmailTemplate.html'")]
        public string SupportEmailTemplatePath { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Reloads the properties from the configXml
        /// </summary>
        protected override void ReloadProperties()
        {
            this.ConnectionString = new ConnectionStringBuilderFacade(this.UnderlyingConfig.ConnectionStrings.ConnectionStrings["SACSEntitiesContainer"]);
            this.GraphMaxPoints = int.Parse("0" + this.GetAppSettingValue("Performance.GraphMaxPoints"));
            this.GraphThreshold = decimal.Parse("0" + this.GetAppSettingValue("Performance.GraphThreshold"), CultureInfo.InvariantCulture);
            this.SupportEmailAddress = this.GetAppSettingValue("System.SupportEmailAddress");
            this.SuccessEmailTemplatePath = this.GetAppSettingValue("System.SuccessEmailTemplate");
            this.SupportEmailTemplatePath = this.GetAppSettingValue("System.SupportEmailTemplate");
            this.MailSettings = this.UnderlyingConfig.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
        }

        /// <summary>
        /// Updates the underlying configuration.
        /// </summary>
        protected override void UpdateUnderlyingConfig()
        {
            // o_O* I really need better "ConnectionString" naming here.
            this.UnderlyingConfig.ConnectionStrings.ConnectionStrings["SACSEntitiesContainer"].ConnectionString = this.ConnectionString.Settings.ConnectionString;
            this.SetAppSettingValue("Performance.GraphMaxPoints", this.GraphMaxPoints.ToString());
            this.SetAppSettingValue("Performance.GraphThreshold", this.GraphThreshold.ToString());
            this.SetAppSettingValue("System.SupportEmailAddress", this.SupportEmailAddress);
            this.SetAppSettingValue("System.SuccessEmailTemplate", this.SuccessEmailTemplatePath);
            this.SetAppSettingValue("System.SupportEmailTemplate", this.SupportEmailTemplatePath);
        }

        /// <summary>
        /// Updates the pickup directory location.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdatePickupDirectoryLocation(string value)
        {
            string newValue = value;
            ////if (this.SmtpDeliveryMethod != System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory)
            ////{
            ////    newValue = null;
            ////}

            this.MailSettings.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation = newValue;
        }

        /// <summary>
        /// Updates the network host.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateNetworkHost(string value)
        {
            string newValue = value;
            ////if (this.SmtpDeliveryMethod != System.Net.Mail.SmtpDeliveryMethod.Network)
            ////{
            ////    newValue = null;
            ////}

            this.MailSettings.Smtp.Network.Host = newValue;
        }

        /// <summary>
        /// Updates the name of the network user.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateNetworkUserName(string value)
        {
            string newValue = value;
            ////if (this.SmtpDeliveryMethod != System.Net.Mail.SmtpDeliveryMethod.Network)
            ////{
            ////    newValue = null;
            ////}

            this.MailSettings.Smtp.Network.UserName = newValue;
        }

        /// <summary>
        /// Updates the network password.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateNetworkPassword(string value)
        {
            string newValue = value;
            ////if (this.SmtpDeliveryMethod != System.Net.Mail.SmtpDeliveryMethod.Network)
            ////{
            ////    newValue = null;
            ////}

            this.MailSettings.Smtp.Network.Password = newValue;
        }

        /// <summary>
        /// Updates the network port.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateNetworkPort(int value)
        {
            int newValue = value;
            ////if (this.SmtpDeliveryMethod != System.Net.Mail.SmtpDeliveryMethod.Network)
            ////{
            ////    newValue = 25;
            ////}

            this.MailSettings.Smtp.Network.Port = newValue;
        }

        #endregion Methods
    }
}