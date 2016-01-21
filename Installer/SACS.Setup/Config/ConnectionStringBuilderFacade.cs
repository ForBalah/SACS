using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace SACS.Setup.Config
{
    /// <summary>
    /// Simplifies the SQL and Entity ConnectionStringBuilder classes for use in the propertygrid
    /// </summary>
    [TypeConverter(typeof(ConnectionStringConverter))]
    public class ConnectionStringBuilderFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringBuilderFacade"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        public ConnectionStringBuilderFacade(ConnectionStringSettings connectionStringSettings)
        {
            this.Settings = connectionStringSettings;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the attach database filename.
        /// </summary>
        /// <value>
        /// The attach database filename.
        /// </value>
        [DisplayName("AttachDbFilename")]
        [DefaultValue("|DataDirectory|\\SACS.mdf")]
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Typical value for this will be \"|DataDirectory|\\SACS.mdf\". Blank this out if connecting to a SQL server and use InitialCatalog instead.")]
        public string AttachDBFilename
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().AttachDBFilename;
            }

            set
            {
                // This process allows us to leverage each builder so that the value can go
                // back into the Settings' ConnectionString robustly.
                var entityPart = this.AsEntityConnectionStringBuilder();
                var sqlPart = this.AsSqlConnectionStringBuilder();
                sqlPart.AttachDBFilename = value; // the payload
                entityPart.ProviderConnectionString = sqlPart.ConnectionString;
                this.Settings.ConnectionString = entityPart.ConnectionString;
            }
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [DisplayName("Data Source")]
        [DefaultValue("(LocalDB)\\v11.0")]
        [RefreshProperties(RefreshProperties.All)]
        [TypeConverter(typeof(ConnectionStringBuilderFacade.SqlDataSourceConverter))]
        [Description("Indicates the name of the data source to connect to")]
        public string DataSource
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().DataSource;
            }

            set
            {
                // This process allows us to leverage each builder so that the value can go
                // back into the Settings' ConnectionString robustly.
                var entityPart = this.AsEntityConnectionStringBuilder();
                var sqlPart = this.AsSqlConnectionStringBuilder();
                sqlPart.DataSource = value; // the payload
                entityPart.ProviderConnectionString = sqlPart.ConnectionString;
                this.Settings.ConnectionString = entityPart.ConnectionString;
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        [Browsable(false)]
        public ConnectionStringSettings Settings { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Casts this facade as the EntityConnectionStringBuilder.
        /// </summary>
        /// <returns></returns>
        public EntityConnectionStringBuilder AsEntityConnectionStringBuilder()
        {
            return new EntityConnectionStringBuilder(this.Settings.ConnectionString);
        }

        /// <summary>
        /// Casts this facade as the SqlConnectionStringBuilder.
        /// </summary>
        /// <returns></returns>
        public SqlConnectionStringBuilder AsSqlConnectionStringBuilder()
        {
            return new SqlConnectionStringBuilder(this.AsEntityConnectionStringBuilder().ProviderConnectionString);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Settings.ToString();
        }

        #endregion Methods

        /// <summary>
        /// Used to convert the DataSource. This is taken from the System.Data disassembly.
        /// </summary>
        private sealed class SqlDataSourceConverter : StringConverter
        {
            private TypeConverter.StandardValuesCollection _standardValues;

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                TypeConverter.StandardValuesCollection standardValuesCollection = this._standardValues;
                if (this._standardValues == null)
                {
                    DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();
                    DataColumn column = dataSources.Columns["ServerName"];
                    DataColumn column2 = dataSources.Columns["InstanceName"];
                    DataRowCollection rows = dataSources.Rows;
                    string[] array = new string[rows.Count];
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text = rows[i][column] as string;
                        string text2 = rows[i][column2] as string;
                        if (text2 == null || text2.Length == 0 || "MSSQLSERVER" == text2)
                        {
                            array[i] = text;
                        }
                        else
                        {
                            array[i] = text + "\\" + text2;
                        }
                    }
                    Array.Sort<string>(array);
                    standardValuesCollection = new TypeConverter.StandardValuesCollection(array);
                    this._standardValues = standardValuesCollection;
                }
                return standardValuesCollection;
            }
        }
    }
}