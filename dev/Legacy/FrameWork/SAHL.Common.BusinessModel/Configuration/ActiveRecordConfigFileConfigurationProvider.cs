namespace SAHL.Common.BusinessModel.Configuration
{
    using SAHL.Common.BusinessModel.Interfaces.Configuration;
    using SAHL.Common.Configuration;

    public class ActiveRecordConfigFileConfigurationProvider : ConfigFileConfigurationProviderBase, IActiveRecordConfigurationProvider
    {
        public ActiveRecordConfigFileConfigurationProvider()
            : base()
        {
        }

        public string ConnectionStringFor2AMDB
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.DBConnectionString"].ConnectionString; }
        }

        public string ConnectionStringForX2DB
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.X2"].ConnectionString; }
        }

        public string ConnectionStringForSAHLDB
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.SAHLConnectionString"].ConnectionString; }
        }

        public string ConnectionStringForWarehouseDB
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.Warehouse"].ConnectionString; }
        }

        public string ConnectionStringForDocumentManagementDB
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.Warehouse"].ConnectionString; }
        }

        public string ConnectionStringForImageIndexDB
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.ImageIndexConnectionString"].ConnectionString; }
        }
    }
}