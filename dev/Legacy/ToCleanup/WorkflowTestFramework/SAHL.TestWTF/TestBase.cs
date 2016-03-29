using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using System.Resources;
using System.IO;
using SAHL.Common.Security;
using NUnit.Framework;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using SAHL.Common.Collections;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories.Strategies;
using SAHL.TestWTF.Strategies;
using SAHL.Common.DataAccess;
using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Utils;

namespace SAHL.TestWTF
{
    /// <summary>
    /// Base class for all SAHL Tests.  This should contain utility methods and properties, for example 
    /// getting a handle on the DBHelper, cleaning up data, etc.
    /// </summary>
    public abstract class TestBase
    {
        public const string ApplicationADUserGroups = "ADUserGroups";

        #region View Constants

        public const string VIEWINIT = "ViewInitialised";
        public const string VIEWLOAD = "ViewLoaded";
        public const string VIEWPRERENDER = "ViewPreRender";

        #endregion

        #region Private Attributes

        private static bool _initialiseErrorRaised = false;
        private static bool IsActiveRecordInitialised = false;      // used to check if we need to initialise ActiveRecord
        private static bool IsActivePrincipalCacheInitialised = false;      // used to check if we need to initialise ActiveRecord
        protected MockRepository _mockery = null;
        private ILookupRepository _lookupRepository = null;
        private TypeFactoryStrategy _strategy = TypeFactoryStrategy.Default;

        private DefaultStrategy defaultStrategy = new DefaultStrategy();
        private MockStrategy mockStrategy = new MockStrategy();

        private string[] _FeatureGroups;
        private bool _useLazyInitialisation;

        private DBHelper _dbHelper = new DBHelper(Databases.TwoAM);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for TestBase.  This will initialise ActiveRecord so it does not have to be done in test classes.
        /// </summary>
        public TestBase(bool UseLazyInitialisation)
        {
            _useLazyInitialisation = UseLazyInitialisation;
            DoSetup();
        }

        public TestBase() : this(true)
        {

        }

        #endregion

        #region Methods

        protected void DoSetup()
        {
            if (!IsActiveRecordInitialised)
                InitialiseActiveRecord();
            _mockery = new MockRepository();
            SetupPrincipalCache(TestPrincipal, true);
            CurrentPrincipalCache.DomainMessages.Clear();
        }

        /// <summary>
        /// Convenience method to delete a single record from a table (used for cleaning up test data).
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="primaryKeyColumn">The primary key column.</param>
        /// <param name="keyValue">The primary key value.</param>
        /// <remarks>This will only work with tables that do not have a composite key.</remarks>
        protected static void DeleteRecord(string tableName, string primaryKeyColumn, string keyValue)
        {
            DeleteRecord(tableName, primaryKeyColumn, keyValue, String.Empty);
        }

        /// <summary>
        /// Convenience method to delete a single record from a table (used for cleaning up test data).
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="primaryKeyColumn">The primary key column.</param>
        /// <param name="keyValue">The primary key value.</param>
        /// <param name="criteria">Any extra criteria to attach to the statement.</param>
        /// <remarks>This will only work with tables that do not have a composite key.</remarks>
        protected static void DeleteRecord(string tableName, string primaryKeyColumn, string keyValue, string criteria)
        {
            string sql = "DELETE FROM " + tableName + " WHERE " + primaryKeyColumn + " = " + keyValue;
            if (criteria.Length > 0)
                sql = sql + " AND " + criteria;

            DBHelper dbHelper = new DBHelper(Databases.TwoAM);
            dbHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// Utility method to get a primary key from a table - this will just return the first available primary 
        /// key value it finds.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyColumn"></param>
        protected object GetPrimaryKey(string tableName, string primaryKeyColumn)
        {
             return GetPrimaryKey(tableName, primaryKeyColumn, String.Empty);
        }

        /// <summary>
        /// Utility method to get a primary key from a table - this will just return the first available primary 
        /// key value it finds.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyColumn"></param>
        /// <param name="criteria">Any criteria for selecting the row, excluding the WHERE keyword e.g. "ParentKey IS NULL".</param>
        /// <returns></returns>
        protected object GetPrimaryKey(string tableName, string primaryKeyColumn, string criteria)
        {
            object key = null;

            // build the sql statement
            string sql = "SELECT TOP 1 " + primaryKeyColumn + " FROM " + tableName;
            if (criteria.Length > 0)
                sql = sql + " WHERE " + criteria;

            // get an id to use for the test
            IDataReader reader = DBHelper.ExecuteReader(sql);
            while (reader.Read())
            {
                key = reader.GetValue(0);
            }
            reader.Close();
            reader.Dispose();

            return key;
        }

        /// <summary>
        /// Initialises the nHibernate setup.
        /// </summary>
        private void InitialiseActiveRecord()
        {
            InPlaceConfigurationSource configSource = null;

            if (_useLazyInitialisation)
                configSource = new InPlaceConfigurationSourceWithLazyDefault();
            else
                configSource = new InPlaceConfigurationSource();

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties.Add("connection.connection_string", Properties.Settings.Default.ConnString);
            properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_2AM<>), properties);

            Dictionary<string, string> properties2 = new Dictionary<string, string>();
            properties2.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties2.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties2.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties2.Add("connection.connection_string", Properties.Settings.Default.X2);
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
            Assembly[] asm = new Assembly[3];
            asm[0] = Assembly.Load("SAHL.Common.BusinessModel.DAO");
            asm[1] = Assembly.Load("SAHL.Common.X2.BusinessModel.DAO");
            asm[2] = Assembly.Load("WTF.DAO");

            try
            {
                ActiveRecordStarter.Initialize(asm, configSource);
                _initialiseErrorRaised = false;
            }
            catch (Exception ex)
            {
                if (!_initialiseErrorRaised)
                {
                    _initialiseErrorRaised = true;
                    throw;
                }
            }

            // set the static variable so initialisation is not tried again
            IsActiveRecordInitialised = true;

        }

        protected string GetSQLResource(Type BaseType, string Name)
        {
            Stream SQLStream = null;
            StreamReader Sr = null;
            try
            {
                SQLStream = BaseType.Assembly.GetManifestResourceStream(BaseType, "Scripts." + Name);
                SQLStream.Flush();
                SQLStream.Seek(0, SeekOrigin.Begin);
                Sr = new StreamReader(SQLStream);
                return Sr.ReadToEnd();
            }
            finally
            {
                Sr.Dispose();
                SQLStream.Dispose();
            }
        }

        /// <summary>
        /// Generic test method to do a basic find on an entity.  This override requires a table name and primary key 
        /// column so a primary key can be retrieved for the test.
        /// </summary>
        /// <returns>The created entity.</returns>
        protected T TestFind<T>(string tableName, string primaryKeyColumn)
        {
            object key = GetPrimaryKey(tableName, primaryKeyColumn);

            // ensure a key has been returned, otherwise there is no data and the test does not run
            if (key == null)
                Assert.Ignore("Unable to find a " + tableName + " record to perform test.");

            return (T)TestFind<T>(key);
        }

        /// <summary>
        /// Generic test method to do a basic find on an entity, given the primary key of the entity.
        /// </summary>
        /// <returns>The created entity.</returns>
        protected object TestFind<T>(object key)
        {
            Type type = typeof(T);

            // retrieve a single entity using the key
            object entity = ActiveRecordMediator.FindByPrimaryKey(type, key);
            Assert.IsNotNull(entity, type.Name + ".Find returned a null object for key " + key.ToString());
            Assert.IsInstanceOfType(type, entity, "Object retrieved does not match type " + type.FullName);

            return entity;
        }

        public void SetupPrincipalCache(SAHLPrincipal principal)
        {
            SetupPrincipalCache(principal, false, null);
        }

        public void SetupPrincipalCache(SAHLPrincipal principal, bool UseMocks)
        {
            SetupPrincipalCache(principal, UseMocks, null);
        }

        public void SetupPrincipalCache(SAHLPrincipal principal, bool UseMocks, string[] FeatureGroups)
        {
            if (IsActivePrincipalCacheInitialised)
                return;

            CacheManager principalStore = CacheFactory.GetCacheManager(SAHLPrincipalCache.CacheManagerName);
            string key = principal.Identity.Name.ToLower();

            if (!principalStore.Contains(key))
            {
                SAHLPrincipalCache principalCache = SAHLPrincipalCache.GetPrincipalCache(principal);
                if (!UseMocks)
                    _FeatureGroups = FeatureGroup.FindAllGroups();
                else
                {
                    if (FeatureGroups != null)
                        _FeatureGroups = FeatureGroups;
                    else
                        _FeatureGroups = new string[1] { "DevTestGroup" };
                }
                foreach (string role in _FeatureGroups)
                {
                    if (TestPrincipal.IsInRole(role))
                    {
                        principalCache.Roles.Add(role);
                    }
                }
                principalStore.Add(key, principalCache);
            }

            ILookupRepository LR = new LookupRepository();

            if (UseMocks)
            {
                CacheManager MCM = CacheFactory.GetCacheManager("MOCK");
                MCM.Add(typeof(ILookupRepository).ToString(), LR);
            }
            else
            {
                // setup the per user data for the session
                // LR.LoadAllContextMenus();
                // LR.LoadAllCBOMenus();
                // intitialise the organisational structure and the users menu access
                // PrincipalHelper.InitialisePrincipal(principal);// , CurrentPrincipalCache.AllowedCBOMenus, CurrentPrincipalCache.AllowedContextMenus, CurrentPrincipalCache.UserOriginationSources);
            }
            IsActivePrincipalCacheInitialised = true;
        }

        public TypeFactoryStrategy GetRepositoryStrategy()
        {
            return _strategy;
        }

        public void SetRepositoryStrategy(TypeFactoryStrategy Strategy)
        {
            _strategy = Strategy;
            switch (Strategy)
            {
                case TypeFactoryStrategy.Default:
                    TypeFactory.SetStrategy(defaultStrategy);
                    break;
                case TypeFactoryStrategy.Mock:
                    TypeFactory.SetStrategy(mockStrategy);
                    break;
            }
        }

        /// <summary>
        /// Adds an origination source to the principal.  If you use this, make sure you also 
        /// call <see cref="RemoveOriginationSourceFromPrincipal(OriginationSources)"/> at the 
        /// end of your test or you could break other tests.
        /// </summary>
        /// <param name="origSource"></param>
        protected void AddOriginationSourceToPrincipal(OriginationSources origSource)
        {
            string hql = "from OrganisationStructureOriginationSource_DAO o where o.OriginationSource.Key = ?";
            SimpleQuery<OrganisationStructureOriginationSource_DAO> q = new SimpleQuery<OrganisationStructureOriginationSource_DAO>(hql, origSource);
            OrganisationStructureOriginationSource_DAO[] origSources = q.Execute();
            if (origSources.Length == 0)
                Assert.Ignore("No origination sources found");

            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IOrganisationStructureOriginationSource o = bmtm.GetMappedType<IOrganisationStructureOriginationSource>(origSources[0]);
            
            if (!CurrentPrincipalCache.UserOriginationSourceKeys.Contains(o.OriginationSource.Key))
                CurrentPrincipalCache.UserOriginationSourceKeys.Add(o.OriginationSource.Key);
        }

        /// <summary>
        /// Removes an origination source from the current principal.  Usually used in conjunction with 
        /// <see cref="AddOriginationSourceToPrincipal(OriginationSources)"/>.
        /// </summary>
        /// <param name="origSource"></param>
        protected void RemoveOriginationSourceFromPrincipal(OriginationSources origSource)
        {
            if (CurrentPrincipalCache.UserOriginationSourceKeys.Contains((int)origSource))
                CurrentPrincipalCache.UserOriginationSourceKeys.Remove((int)origSource);
            //foreach (IOrganisationStructureOriginationSource o in CurrentPrincipalCache.UserOriginationSources)
            //{
            //    if (o.Key == (int)origSource)
            //    {
            //        CurrentPrincipalCache.UserOriginationSources.Remove(o);
            //        return;
            //    }
            //}
        }

        public void ClearMockCache()
        {
            MockCache.Flush();
        }

        public DataTable GetQueryResults(string query)
        {
            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection();
            DataTable DT = new DataTable();
            SAHL.Common.DataAccess.Helper.FillFromQuery(DT, query, con, parameters);
            return DT;
        }

        public DataTable GetQueryResults(string query, IDbConnection con)
        {
            ParameterCollection parameters = new ParameterCollection();
            DataTable DT = new DataTable();
            SAHL.Common.DataAccess.Helper.FillFromQuery(DT, query, con, parameters);
            return DT;
        }

        public DataTable GetQueryResultsFromSAHLDB(string query)
        {
            ParameterCollection parameters = new ParameterCollection();
            string conString = Helper.GetSAHLConnectionString();
            IDbConnection con = Helper.GetSQLDBConnectionFromConnectionString(conString);
            DataTable DT = new DataTable();
            SAHL.Common.DataAccess.Helper.FillFromQuery(DT, query, con, parameters);
            return DT;
        }

      /// <summary>
      /// Sets up all the calls required when iterating over a collection using Foreach(obj in collection)
      /// </summary>
      /// <typeparam name="T">The Type (EG IAccount)</typeparam>
      /// <param name="collection">Collection of objects to iterate over.</param>
      /// <param name="current">The first object in the collection</param>
      /// <returns></returns>
      protected void SetupIEnumerator<T>(IEnumerable<T> collection, ref T current)
      {
        IEnumerator<T> enumerator = _mockery.CreateMock<IEnumerator<T>>();
        SetupResult.For(collection.GetEnumerator()).Return(enumerator);
        SetupResult.For(enumerator.MoveNext()).Return(true);
        SetupResult.For(enumerator.Current).IgnoreArguments().Return(current);
        enumerator.Dispose();
        //return enumerator;
      }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a reference to the DBHelper to be used for querying the database.
        /// </summary>
        protected DBHelper DBHelper
        {
            get
            {
                return _dbHelper;
            }
        }

        protected SAHLPrincipalCache CurrentPrincipalCache
        {
            get
            {
                return SAHLPrincipalCache.GetPrincipalCache(TestPrincipal);
            }
        }

        /// <summary>
        /// Gets a reference to the lookup repository.  If you want to use a mocked lookup repository, you 
        /// will need to ensure the strategy is <see cref="TypeFactoryStrategy.Mock"/> by calling <see cref="SetRepositoryStrategy"/>.
        /// </summary>
        public ILookupRepository LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                {
                    if (_strategy == TypeFactoryStrategy.Mock)
                    {
                        _lookupRepository = _mockery.CreateMock<ILookupRepository>();
                        MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
                    }
                    else
                    {
                        _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                    }
                }
                return _lookupRepository;
            }
        }

        /// <summary>
        /// Gets the mock cache object.
        /// </summary>
        public CacheManager MockCache
        {
            get
            {
                return CacheFactory.GetCacheManager("MOCK");
            }
        }

        /// <summary>
        /// Gets the security principal to be used for the tests.
        /// </summary>
        public SAHLPrincipal TestPrincipal
        {
            get
            {
                return new SAHLPrincipal(WindowsIdentity.GetCurrent());
            }
        }

        #endregion

        #region IView Helpers

        /// <summary>
        /// Hook up life cycle events and return the view initialized event raiser.
        /// </summary>
        /// <returns>The view initialized event raiser.</returns>
        protected IDictionary<string, IEventRaiser> LifeCycleEventsCommon(IViewBase View)
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            SetupResult.For(View.Messages).Return(new DomainMessageCollection());

            View.ViewInitialised += null;
            IEventRaiser eventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("ViewInitialised", eventRaiser);

            View.ViewLoaded += null;
            eventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("ViewLoaded", eventRaiser);

            View.ViewPreRender += null;
            eventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("ViewPreRender", eventRaiser);

            return dict;
        }

        #endregion
    }

    public enum TypeFactoryStrategy
    {
        Default,
        Mock
    }
}
