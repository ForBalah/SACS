using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return string.Format("{0}\\{1}", sqlPart.DataSource, string.IsNullOrWhiteSpace(sqlPart.InitialCatalog) ? sqlPart.AttachDBFilename : sqlPart.InitialCatalog);
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
        [Description("The connection string used to connect to the datasource. Default is \"(LocalDB)\v11.0\"")]
        public ConnectionStringBuilderFacade ConnectionString { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Reloads the properties from the configXml
        /// </summary>
        protected override void ReloadProperties()
        {
            this.ConnectionString = new ConnectionStringBuilderFacade(this.UnderlyingConfig.ConnectionStrings.ConnectionStrings["SACSEntitiesContainer"]);
        }

        /// <summary>
        /// Updates the underlying configuration.
        /// </summary>
        protected override void UpdateUnderlyingConfig()
        {
            // o_O* I really need better names here.
            this.UnderlyingConfig.ConnectionStrings.ConnectionStrings["SACSEntitiesContainer"].ConnectionString = this.ConnectionString.Settings.ConnectionString;
        }

        #endregion Methods
    }
}