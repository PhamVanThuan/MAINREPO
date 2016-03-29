// -----------------------------------------------------------------------
// <copyright file="ActiveRecordInitialiser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SAHL.Common.BusinessModel.Config
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework.Config;
    using SAHL.Common.BusinessModel.Interfaces.Configuration;

    public class ActiveRecordInitialiser : IActiveRecordInitialiser
    {
        private InPlaceConfigurationSource configSource;
        private IActiveRecordConfigurationProvider configurationProvider;

        public ActiveRecordInitialiser(IActiveRecordConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public void InitialiseActiveRecord()
        {
            try
            {
                configSource = new InPlaceConfigurationSource();

                Dictionary<string, string> properties = new Dictionary<string, string>();
                properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties.Add("connection.connection_string", configurationProvider.ConnectionStringFor2AMDB);
                properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_2AM<>), properties);

                Dictionary<string, string> properties2 = new Dictionary<string, string>();
                properties2.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties2.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties2.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties2.Add("connection.connection_string", configurationProvider.ConnectionStringForX2DB);
                properties2.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Database.DB_X2<>), properties2);

                Dictionary<string, string> properties3 = new Dictionary<string, string>();
                properties3.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties3.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties3.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties3.Add("connection.connection_string", configurationProvider.ConnectionStringForSAHLDB);
                properties3.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_SAHL<>), properties3);

                Dictionary<string, string> properties4 = new Dictionary<string, string>();
                properties4.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties4.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties4.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties4.Add("connection.connection_string", configurationProvider.ConnectionStringForWarehouseDB);
                properties4.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Warehouse<>), properties4);

                Dictionary<string, string> properties5 = new Dictionary<string, string>();
                properties5.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties5.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties5.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties5.Add("connection.connection_string", configurationProvider.ConnectionStringForImageIndexDB);
                properties5.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_ImageIndex<>), properties5);

                // load the business model assembly(ies) and initialise
                Assembly[] asm = new Assembly[2];
                asm[0] = Assembly.Load("SAHL.Common.BusinessModel.DAO");
                asm[1] = Assembly.Load("SAHL.Common.X2.BusinessModel.DAO");

                ActiveRecordStarter.Initialize(asm, configSource);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
        }
    }
}