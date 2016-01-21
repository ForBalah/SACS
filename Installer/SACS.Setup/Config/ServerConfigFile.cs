using System;
using System.Collections.Generic;
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
        public string DatabaseLocation
        {
            get
            {
                var entityPart = new EntityConnectionStringBuilder(this.DefaultConnectionString.ConnectionString);
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
        public ConnectionStringSettings DefaultConnectionString { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Reloads the properties from the configXml
        /// </summary>
        protected override void ReloadProperties()
        {
            this.DefaultConnectionString = this.UnderlyingConfig.ConnectionStrings.ConnectionStrings["SACSEntitiesContainer"];
        }

        #endregion Methods
    }
}