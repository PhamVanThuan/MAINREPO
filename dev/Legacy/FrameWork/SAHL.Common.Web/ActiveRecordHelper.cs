using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord.Framework.Scopes;
using SAHL.Common.Utils;

namespace SAHL.Common.Web
{
    public sealed class ActiveRecordHelper
    {
        /// <summary>
        /// Initialises ActiveRecord. This Class was created to service The DotNetNuke Web Application on www.sahomeloans.com
        /// </summary>
        public static void InitialiseActiveRecord()
        {
            InPlaceConfigurationSourceWithLazyDefault configSource = new InPlaceConfigurationSourceWithLazyDefault();
            configSource.IsRunningInWebApp = true;
            configSource.ThreadScopeInfoImplementation = typeof(WebThreadScopeInfo);

            try
            {
                Dictionary<string, string> properties = new Dictionary<string, string>();
                properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties.Add("connection.connection_string", Properties.Settings.Default.DBConnectionString);
                properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_2AM<>), properties);

                Dictionary<string, string> properties2 = new Dictionary<string, string>();
                properties2.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties2.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties2.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties2.Add("connection.connection_string", Properties.Settings.Default.X2ConnectionString);
                properties2.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Database.DB_X2<>), properties2);

                Dictionary<string, string> properties3 = new Dictionary<string, string>();
                properties3.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties3.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties3.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties3.Add("connection.connection_string", Properties.Settings.Default.SAHLConnectionString);
                properties3.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_SAHL<>), properties3);

                Dictionary<string, string> properties4 = new Dictionary<string, string>();
                properties4.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties4.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties4.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties4.Add("connection.connection_string", Properties.Settings.Default.WarehouseConnectionString);
                properties4.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Warehouse<>), properties4);

                Dictionary<string, string> properties5 = new Dictionary<string, string>();
                properties5.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties5.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties5.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties5.Add("connection.connection_string", Properties.Settings.Default.ImageIndexConnectionString);
                properties5.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_ImageIndex<>), properties5);

                Dictionary<string, string> properties6 = new Dictionary<string, string>();
                properties6.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties6.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties6.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties6.Add("connection.connection_string", Properties.Settings.Default.TestWTFConnectionString);
                properties6.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Test_WTF<>), properties6);

                // load the business model assembly(ies) and initialise
                Assembly[] asm = new Assembly[2];
                asm[0] = Assembly.Load("SAHL.Common.BusinessModel.DAO");
                asm[1] = Assembly.Load("SAHL.Common.X2.BusinessModel.DAO");

                ActiveRecordStarter.Initialize(asm, configSource);
            }
            catch
            {
                throw;
            }
        }
    }
}