using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord.Framework.Scopes;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Utils;
using System.Configuration;
using SAHL.Communication;
using SAHL.Common.Logging;


namespace SAHL.Common.Web
{
    /// <summary>
    /// Overrides the standard HttpApplication
    /// </summary>
    public class SAHLHttpApplication : HttpApplication
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SAHLHttpApplication()
        {
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            InitialiseActiveRecord();
            InitServiceBus();


            // cache all the lookups

            //#if !DEBUG
            //            InitialiseLookups();
            //#endif
        }

        

        protected void Session_Start(object sender, EventArgs e)
        {
            // setup the per user data for the session
            // ILookupRepository Lookups = TypeFactory.CreateType<ILookupRepository>();

            // this looks a little strange here as it should happen in Application_Start, but as the domain objects
            // require a current SAHLPrincipal, it falls over at that stage - it's not a performance hit though
            // because the lookup repository will not reload the objects if they've already been loaded anyway
            // Lookups.LoadAllContextMenus();
            // Lookups.LoadAllCBOMenus();

            // initialise the organisational structure
            //PrincipalHelper.InitialisePrincipal(CurrentPrincipal);
            // , CurrentPrincipalCache.AllowedCBOMenus, CurrentPrincipalCache.AllowedContextMenus, CurrentPrincipalCache.UserOriginationSources);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // clear the per user data for the session
            //PrincipalHelper.DisposePrincipal(CurrentPrincipal);
        }

        #region Public Properties

        /// <summary>
        /// Returns the current principal cast as a <see cref="SAHLPrincipal"/> object.
        /// </summary>
        public SAHLPrincipal CurrentPrincipal
        {
            get
            {
                if (this.Context == null || this.Context.User == null)
                    return null;

                SAHLPrincipal principal = this.Context.User as SAHLPrincipal;
                return principal;
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Initialises ActiveRecord.
        /// </summary>
        private void InitialiseActiveRecord()
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

        private void InitServiceBus()
        {
            IMessageBus messageBus = new EasyNetQMessageBus(new EasyNetQMessageBusConfigurationProvider());
            HttpContext.Current.Application.Add("IMessagebus", messageBus);

            LogPlugin.Logger = new MessageBusLogger(messageBus, new MessageBusLoggerConfiguration());
            MetricsPlugin.Metrics = new MessageBusMetrics(messageBus, new MessageBusMetricsConfiguration());
        }

        //private void InitialiseLookups()
        //{
        //    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();

        //    Type t = LR.GetType();
        //    PropertyInfo[] props = t.GetProperties();
        //    foreach (PropertyInfo pi in props)
        //    {
        //        MethodInfo mi = pi.GetGetMethod();
        //        mi.Invoke(LR, null);
        //    }
        //}

        #endregion Private Methods
    }
}