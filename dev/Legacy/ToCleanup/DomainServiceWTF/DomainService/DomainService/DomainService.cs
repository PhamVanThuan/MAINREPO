using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Remoting;
using X2DomainService.Interface.Origination;
using System.Runtime.Remoting.Channels.Tcp;
using SAHL.X2.Common;
using SAHL.Common.Security;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Castle.ActiveRecord.Framework.Config;
using System.Reflection;
using Castle.ActiveRecord;
using System.Security.Principal;
using System.Threading;
using System.Configuration;
using System.Runtime.Serialization.Formatters;
using SAHL.Common.CacheData;

namespace DomainService
{
    public class X2DomainService
    {
        Dictionary<string, object> Services = new Dictionary<string, object>();
        ICommon common = null;
        //IApplicationCapture AppCap = null;
        object AppCap = null;
        IFL fl = null;
        IApplicationManagement AppMan = null;
        ICredit cred = null;
        IValuations val = null;
        IQC qc = null;

        #region AR
        static InPlaceConfigurationSource configSource;
        static public void InitialiseActiveRecord()
        {
            configSource = new InPlaceConfigurationSource();

            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Dictionary<string, string> properties = new Dictionary<string, string>();
                properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties.Add("connection.connection_string", config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.DBConnectionString"].ConnectionString);
                properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_2AM<>), properties);

                Dictionary<string, string> properties2 = new Dictionary<string, string>();
                properties2.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties2.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties2.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties2.Add("connection.connection_string", config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.X2"].ConnectionString);
                properties2.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Database.DB_X2<>), properties2);

                Dictionary<string, string> properties3 = new Dictionary<string, string>();
                properties3.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties3.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties3.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties3.Add("connection.connection_string", config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.SAHLConnectionString"].ConnectionString);
                properties3.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_SAHL<>), properties3);


                Dictionary<string, string> properties4 = new Dictionary<string, string>();
                properties4.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties4.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties4.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties4.Add("connection.connection_string", config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.Warehouse"].ConnectionString);
                properties4.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Warehouse<>), properties4);

                Dictionary<string, string> properties5 = new Dictionary<string, string>();
                properties5.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties5.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                properties5.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties5.Add("connection.connection_string", config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.Warehouse"].ConnectionString);
                properties5.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_ImageIndex<>), properties5);


                Dictionary<string, string> properties6 = new Dictionary<string, string>();
                properties6.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                properties6.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
                properties6.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                properties6.Add("connection.connection_string", config.ConnectionStrings.ConnectionStrings["SAHL.Common.Properties.Settings.TestWTFConnectionString"].ConnectionString);
                properties6.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Test_WTF<>), properties6);

                // load the business model assembly(ies) and initialise
                Assembly[] asm = new Assembly[3];
                asm[0] = Assembly.Load("SAHL.Common.BusinessModel.DAO");
                asm[1] = Assembly.Load("SAHL.Common.X2.BusinessModel.DAO");
                asm[2] = Assembly.Load("WTF.DAO");

                ActiveRecordStarter.Initialize(asm, configSource);

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
        }

        static public void SetupPrincipalCache(SAHLPrincipal Principal)
        {
            SAHLPrincipal pcp = new SAHLPrincipal(new GenericIdentity("X2"));
            Thread.CurrentPrincipal = pcp;
            string key = pcp.Identity.Name.ToLower();

            CacheManager principalStore = CacheFactory.GetCacheManager("SAHLPrincipalStore");
            SAHLPrincipalCache principalCache = SAHLPrincipalCache.GetPrincipalCache(pcp);
            principalStore.Add(key.ToLower(), principalCache);
            principalCache.DomainMessages.Clear();
            Thread.CurrentPrincipal = pcp;
        }

        static SAHLPrincipalCache CurrentPrincipalCache
        {
            get
            {
                CacheManager principalStore = CacheFactory.GetCacheManager("SAHLPrincipalStore");
                string key = TestPrincipal.Identity.Name;
                return principalStore[key] as SAHLPrincipalCache;
            }
        }

        static public SAHLPrincipal TestPrincipal
        {
            get
            {
                return new SAHLPrincipal(WindowsIdentity.GetCurrent());
            }
        }
        internal static void SetThreadPrincipal(string ADUser)
        {
            SAHLPrincipal pcp = new SAHLPrincipal(new GenericIdentity("X2"));
            Thread.CurrentPrincipal = pcp;
            string key = pcp.Identity.Name.ToLower();

            CacheManager principalStore = CacheFactory.GetCacheManager("SAHLPrincipalStore");
            SAHLPrincipalCache principalCache = SAHLPrincipalCache.GetPrincipalCache(pcp);
            principalStore.Add(key.ToLower(), principalCache);
            principalCache.DomainMessages.Clear();
            Thread.CurrentPrincipal = pcp;
        }

        internal static void ClearThreadPrincipal()
        {
            Thread.CurrentPrincipal = null;
        }
        #endregion

        public X2DomainService()
        {
            LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;
            LogSettingsClass lsl = new LogSettingsClass();
            lsl.AppName = "X2 Domain Service";
            lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
            lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
            lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
            lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
            lsl.FilePath = section.Logging["File"].path;
            lsl.NumDaysToStore = 14;
            lsl.RollOverSizeInKB = 2048;
            LogPlugin.SeedLogSettings(lsl);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string AssemblyName = args.Name;
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            // Use FullName Type Resolve
            foreach (Assembly asm in asms)
            {
                if (asm.FullName == AssemblyName)
                    return asm;
            }
            // Use simple name resolution. This may cause the TypeFactoryCreate to throw if the asm is the wrong one (ie wrong metadata)
            foreach (Assembly asm in asms)
            {
                int pos = asm.FullName.IndexOf(',');
                string tn = asm.FullName.Substring(0, pos);
                if (tn == AssemblyName)
                    return asm;
            }
            LogPlugin.LogWarning("Unable to RESOLVE assembly:{0}", args.Name);
            return null;
        }

        private void Create(int Port, string Name, string URI, Type Interface, Type Concrete)
        {
            LogPlugin.LogInfo("Binding to {0}:{1}", Name, Port);
#warning Perhaps try singlecall? Potential perf hit?
            RemotingConfiguration.RegisterWellKnownServiceType(
                Concrete,
                Name, WellKnownObjectMode.Singleton);

            object o = Activator.GetObject(Interface, URI);
            Services.Add(Name, o);
        }

        private void Register(DomainServiceSection Section)
        {
            ServiceElement se = null;
            string TypeName = "";
            for (int j = 0; j < Section.DomainServices.Count; j++)
            {
                Assembly Asm = null;
                // try load the type and extract repositories
                se = Section.DomainServices[j];
                try
                {
                    Asm = Assembly.Load(se.AssemblyName);
                    Type[] _Types = Asm.GetTypes();
                    for (int k = 0; k < _Types.Length; k++)
                    {
                        ServiceAttribute[] sas = _Types[k].GetCustomAttributes(typeof(ServiceAttribute), false) as ServiceAttribute[];
                        if (sas.Length == 1)
                        {
                            try
                            {
                                ServiceAttribute sa = sas[0];
                                string AssemblyFullName = Asm.FullName;
                                string URI = sa.GetURL(se.IP);
                                Create(sa.Port, sa.Name, URI, sa.Interface, sa.Concrete);
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException rtle)
                {
                    throw;
                }
                catch (Exception E)
                {
                    throw new Exception(string.Format("Error loading types from asm:{0}", se.AssemblyName), E);
                }
            }
        }

        public void OnStart(string[] args)
        {
            try
            {
                InitialiseActiveRecord();
                LogPlugin.LogError("OnStart");
                //RegisterCommon();
                //RegisterApplicationCapture();
                //RegisterFL();
                //RegisterValuations();
                //RegisterCredit();
                //RegisterAppMan();
                //RegisterQC();
                BinaryClientFormatterSinkProvider clientProvider = null;
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                DomainServiceSection Section = ConfigurationManager.GetSection("DomainServices") as DomainServiceSection;
                IDictionary dict = new Hashtable();
                dict["port"] = 10000;
                dict["name"] = "Channel";
                dict["typeFilterLevel"] = TypeFilterLevel.Full;
                
                TcpChannel Ch = new TcpChannel(dict, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(Ch, false);
                Register(Section);

                LogPlugin.LogInfo("OnStart - Started");
            }
            catch (Exception ex)
            {
                LogPlugin.LogError(ex.ToString());
            }
        }

        public void OnStop()
        {
            IChannel Ch = ChannelServices.GetChannel("ApplicationManagement");
            if (Ch != null)
            {
                ChannelServices.UnregisterChannel(Ch);
            }
            Ch = ChannelServices.GetChannel("Common");
            if (Ch != null)
            {
                ChannelServices.UnregisterChannel(Ch);
            }
        }
    }
}
