using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SACS.Setup.Config
{
    /// <summary>
    /// Simplifies the SQL and Entity ConnectionStringBuilder classes for use in the propertygrid
    /// </summary>
    [TypeConverter(typeof(ConnectionStringConverter))]
    public class ConnectionStringBuilderFacade
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringBuilderFacade"/> class.
        /// </summary>
        /// <param name="connectionStringSettings">The connection string settings.</param>
        public ConnectionStringBuilderFacade(ConnectionStringSettings connectionStringSettings)
        {
            this.Settings = connectionStringSettings;
        }

        #endregion Constructors and Destructors

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
                this.SetSqlConnectionValue("AttachDBFilename", value);
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
        [Description("Indicates the name of the data source to connect to.")]
        public string DataSource
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().DataSource;
            }

            set
            {
                this.SetSqlConnectionValue("DataSource", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the database associated with the connection.
        /// </summary>
        /// <value>
        /// The value of the System.Data.SqlClient.SqlConnectionStringBuilder.InitialCatalog
        /// property, or String.Empty if none has been supplied.
        /// </value>
        [DisplayName("Initial Catalog")]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.All)]
        [TypeConverter(typeof(ConnectionStringBuilderFacade.SqlInitialCatalogConverter))]
        [Description("The name of the initial catalog or database in the data source.")]
        public string InitialCatalog
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().InitialCatalog;
            }

            set
            {
                // special case: it takes DBNull, not a normal null
                this.SetSqlConnectionValue("InitialCatalog", value == null ? System.DBNull.Value.ToString() : value);
            }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether User ID and Password
        /// are specified in the connection (when false) or whether the current Windows
        /// account credentials are used for authentication (when true).
        /// </summary>
        /// <value>
        ///   The value of the System.Data.SqlClient.SqlConnectionStringBuilder.IntegratedSecurity
        ///   property, or false if none has been supplied.
        /// </value>
        [DisplayName("Integrated Security")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Whether the connection is to be a secure connection or not.")]
        public bool IntegratedSecurity
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().IntegratedSecurity;
            }

            set
            {
                this.SetSqlConnectionValue("IntegratedSecurity", value);
            }
        }

        /// <summary>
        /// Gets or sets the password for the SQL Server account.
        /// </summary>
        /// <value>
        /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.Password" /> property, or String.Empty if none has been supplied.
        /// </value>
        [DisplayName("Password")]
        [DefaultValue("")]
        [PasswordPropertyText(true)]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Indicates the password to be used when connecting to the data source.")]
        public string Password
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().Password;
            }

            set
            {
                this.SetSqlConnectionValue("Password", value);
            }
        }

        /// <summary>
        /// Gets or sets the user ID to be used when connecting to SQL Server.
        /// </summary>
        /// <value>
        /// The value of the <see cref="P:System.Data.SqlClient.SqlConnectionStringBuilder.UserID" /> property, or String.Empty if none has been supplied.
        /// </value>
        [DisplayName("User ID")]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Indicates the user ID to be used when connecting to the data source.")]
        public string UserId
        {
            get
            {
                return this.AsSqlConnectionStringBuilder().UserID;
            }

            set
            {
                this.SetSqlConnectionValue("UserID", value);
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

        /// <summary>
        /// Sets the value on the property belogning to the SQL connection.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        private void SetSqlConnectionValue(string property, object value)
        {
            // This process allows us to leverage each builder so that the value can go
            // back into the Settings' ConnectionString robustly.
            var entityPart = this.AsEntityConnectionStringBuilder();
            var sqlPart = this.AsSqlConnectionStringBuilder();
            sqlPart.GetType().GetProperty(property).SetValue(sqlPart, value); // the payload
            entityPart.ProviderConnectionString = sqlPart.ConnectionString;
            this.Settings.ConnectionString = entityPart.ConnectionString;
        }

        #endregion Methods

        #region Converters

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

        /// <summary>
        /// Used to convert the InitialCatalog. This is tken from the System.Data dissasembly.
        /// </summary>
        private sealed class SqlInitialCatalogConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return this.GetStandardValuesSupportedInternal(context);
            }

            private bool GetStandardValuesSupportedInternal(ITypeDescriptorContext context)
            {
                bool result = false;
                if (context != null)
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = context.Instance as SqlConnectionStringBuilder;
                    if (sqlConnectionStringBuilder != null && 0 < sqlConnectionStringBuilder.DataSource.Length && (sqlConnectionStringBuilder.IntegratedSecurity || 0 < sqlConnectionStringBuilder.UserID.Length))
                    {
                        result = true;
                    }
                }

                return result;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                if (this.GetStandardValuesSupportedInternal(context))
                {
                    List<string> list = new List<string>();
                    try
                    {
                        SqlConnectionStringBuilder sqlConnectionStringBuilder = (SqlConnectionStringBuilder)context.Instance;
                        using (SqlConnection sqlConnection = new SqlConnection())
                        {
                            sqlConnection.ConnectionString = sqlConnectionStringBuilder.ConnectionString;
                            sqlConnection.Open();
                            DataTable schema = sqlConnection.GetSchema("DATABASES");
                            foreach (DataRow dataRow in schema.Rows)
                            {
                                string item = (string)dataRow["database_name"];
                                list.Add(item);
                            }
                        }
                    }
                    catch (SqlException)
                    {
                        // ADP.TraceExceptionWithoutRethrow(e); // no access to this class.
                    }

                    return new TypeConverter.StandardValuesCollection(list);
                }

                return null;
            }
        }

        #endregion Converters
    }
}