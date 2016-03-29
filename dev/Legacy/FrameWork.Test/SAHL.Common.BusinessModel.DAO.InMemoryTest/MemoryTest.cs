using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using NHibernate;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.CacheData;
using SAHL.Common.DataAccess;
using SAHL.Common.Security;
using SAHL.Common.Test.NHibernateSQLiteDriver;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel.DAO.InMemoryTest
{
    public class InMemoryTestBase
    {
        public bool InitializeErrorRaised { get; set; }

        public bool UseLazyInitialisation { get; set; }

        public static bool IsActiveRecordInitialised { get; set; }

        public InMemoryTestBase()
        {
        }

        protected virtual void InitialiseActiveRecord()
        {
            if (IsActiveRecordInitialised)
                return;

            InPlaceConfigurationSource configSource = null;

            if (UseLazyInitialisation)
                configSource = new InPlaceConfigurationSourceWithLazyDefault();
            else
                configSource = new InPlaceConfigurationSource();

            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("connection.driver_class", "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteDriver, SAHL.Common.Test");

            //properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            properties.Add("connection.provider", "SAHL.Common.BusinessModel.DAO.InMemoryTest.InMemoryTestProvider, SAHL.Common.BusinessModel.DAO.InMemoryTest");
            properties.Add("connection.connection_string", "Data Source=:memory:;Version=3;New=True");
            properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties.Add("query.substitutions", "true=1;false=0");
            properties.Add("show_sql", "true");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_2AM<>), properties);

            Dictionary<string, string> properties2 = new Dictionary<string, string>();
            properties2.Add("connection.driver_class", "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteDriver, SAHL.Common.Test");

            //properties2.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            properties2.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            properties2.Add("connection.provider", "SAHL.Common.BusinessModel.DAO.InMemoryTest.InMemoryTestProvider, SAHL.Common.BusinessModel.DAO.InMemoryTest");
            properties2.Add("connection.connection_string", "Data Source=:memory:;Version=3;New=True");
            properties2.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties2.Add("query.substitutions", "true=1;false=0");
            properties2.Add("show_sql", "true");
            configSource.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Database.DB_X2<>), properties2);

            Dictionary<string, string> properties3 = new Dictionary<string, string>();

            //properties3.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            properties3.Add("connection.driver_class", "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteDriver, SAHL.Common.Test");
            properties3.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            properties3.Add("connection.provider", "SAHL.Common.BusinessModel.DAO.InMemoryTest.InMemoryTestProvider, SAHL.Common.BusinessModel.DAO.InMemoryTest");
            properties3.Add("connection.connection_string", "Data Source=:memory:;Version=3;New=True");
            properties3.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties3.Add("query.substitutions", "true=1;false=0");
            properties3.Add("show_sql", "true");

            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_SAHL<>), properties3);

            Dictionary<string, string> properties4 = new Dictionary<string, string>();
            properties4.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties4.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties4.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties4.Add("connection.connection_string", System.Configuration.ConfigurationManager.ConnectionStrings["SAHL.Test.Properties.Settings.WarehouseConnectionString"].ConnectionString);
            properties4.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Warehouse<>), properties4);

            Dictionary<string, string> properties5 = new Dictionary<string, string>();
            properties5.Add("connection.driver_class", "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteDriver, SAHL.Common.Test");

            //properties5.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            properties5.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            properties5.Add("connection.provider", "SAHL.Common.BusinessModel.DAO.InMemoryTest.InMemoryTestProvider, SAHL.Common.BusinessModel.DAO.InMemoryTest");
            properties5.Add("connection.connection_string", "Data Source=:memory:;Version=3;New=True");
            properties5.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties5.Add("query.substitutions", "true=1;false=0");
            properties5.Add("show_sql", "true");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_ImageIndex<>), properties5);

            Dictionary<string, string> properties6 = new Dictionary<string, string>();
            properties6.Add("connection.driver_class", "SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteDriver, SAHL.Common.Test");

            //properties6.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            properties6.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            properties6.Add("connection.provider", "SAHL.Common.BusinessModel.DAO.InMemoryTest.InMemoryTestProvider, SAHL.Common.BusinessModel.DAO.Test");
            properties6.Add("connection.connection_string", "Data Source=:memory:;Version=3;New=True");
            properties6.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties6.Add("query.substitutions", "true=1;false=0");
            properties6.Add("show_sql", "true");
            configSource.Add(typeof(SAHL.Common.BusinessModel.DAO.Database.DB_Test_WTF<>), properties6);

            // load the business model assembly(ies) and initialise
            Assembly[] asm = new Assembly[2];
            asm[0] = Assembly.Load("SAHL.Common.BusinessModel.DAO");
            asm[1] = Assembly.Load("SAHL.Common.X2.BusinessModel.DAO");

            try
            {
                ActiveRecordStarter.Initialize(asm, configSource);
                InitializeErrorRaised = false;
            }
            catch (Exception ex)
            {
                if (!UseLazyInitialisation)
                {
                    UseLazyInitialisation = true;
                    throw;
                }
            }

            // set the static variable so initialisation is not tried again
            IsActiveRecordInitialised = true;
        }
    }

    /// <summary>
    /// In Memory Test Provider
    /// </summary>
    public class InMemoryTestProvider : NHibernate.Connection.DriverConnectionProvider
    {
        public static IDbConnection Connection = null;

        /// <summary>
        /// Get Connection
        /// </summary>
        /// <returns></returns>
        public override System.Data.IDbConnection GetConnection()
        {
            if (Connection == null)
                Connection = base.GetConnection();
            return Connection;
        }

        /// <summary>
        /// Close Connection
        /// </summary>
        /// <param name="conn"></param>
        public override void CloseConnection(System.Data.IDbConnection conn)
        {
            //Do nothing
        }
    }

    [TestFixture]
    public class MemoryTest : InMemoryTestBase
    {
        private static bool dbCreated;
        private static DataTable DT;

        private static object ProcessNonQuery(NHibernate.ISession session, object data)
        {
            IDbConnection conn = session.Connection;
            IDbCommand cmd = data as IDbCommand;
            cmd.Connection = conn;
            session.Transaction.Enlist(cmd);
            return cmd.ExecuteNonQuery();
        }

        public static int Main(string[] args)
        {
            return 0;
        }

        public MemoryTest()
        {
            GenerateTableScript();
            InitialiseActiveRecord();
            CreateDB();
        }

        [SetUp]
        public void Setup()
        {        }

        #region FullDAOLoadSaveLoadTest

        [Test, TestCaseSource(typeof(DAOProvider), "GetDAOTypes")]
        public void FullDAOLoadSaveLoadTest(Type daoType)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            StringBuilder sb = new StringBuilder();

            //this is bad, but was never tested. We
            if (daoType.GetCustomAttributes(typeof(SAHL.Common.BusinessModel.DAO.Attributes.DoNotTestWithGenericTestAttribute), true).Length > 0)
            {
                return;
            }
            try
            {
                using (new TransactionScope())
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Testing ({0})", daoType.Name));
                    object daoInstance = Activator.CreateInstance(daoType);
                    SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.LoadSaveLoad(daoInstance, "Key", false);
                    SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                    Assert.Pass(string.Format("Tested ({0})", daoType.Name));
                }
            }
            catch (NUnit.Framework.SuccessException)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Test Passed ({0})", daoType.Name));
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Test Failed ({0}) {1} {2}", daoType.Name, System.Environment.NewLine, e.ToString()));
                sb.AppendLine(e.ToString());
                sb.AppendLine(e.InnerException != null ? e.InnerException.ToString() : string.Empty);
                sb.AppendLine(string.Format(daoType.Name));
                SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
            }
            spc.DomainMessages.Clear();

            if (sb.Length > 0)
                Assert.Fail(sb.ToString());
        }

        #endregion FullDAOLoadSaveLoadTest

        #region SpecificDAOTest

        //[Ignore("Use when testing specific dao that are failing")]
        [Test]
        public void SpecificDAOTest()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                List<Type> daos = new List<Type>();
                daos.Add(typeof(AccountIndicationType_DAO));

                foreach (var daoType in daos)
                {
                    try
                    {
                        using (new TransactionScope())
                        {
                            System.Diagnostics.Debug.WriteLine("testing - {0}", daoType.Name);
                            object daoInstance = Activator.CreateInstance(daoType);
                            SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.LoadSaveLoad(daoInstance, "Key", false);
                            SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                        }
                    }
                    catch (ActiveRecordException aex)
                    {
                        System.Diagnostics.Debug.WriteLine("failed - {0}", daoType.Name);
                        sb.AppendLine(string.Format(daoType.Name));
                        System.Diagnostics.Debug.WriteLine(aex.Message);
                        System.Diagnostics.Debug.WriteLine(aex.StackTrace);
                        SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("failed - {0}", daoType.Name);
                        sb.AppendLine(string.Format(daoType.Name));
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                    }
                }
            }
            catch (ActiveRecordException aex)
            {
                System.Diagnostics.Debug.WriteLine(aex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            if (sb.Length > 0)
                Assert.Fail(sb.ToString());
        }

        #endregion SpecificDAOTest

        #region FullDAOTestWithLogging

        [Ignore("Use when runnning on local pc to the see the a more descriptive information in a text file")]
        [Test]
        public void FullDAOTestWithLogging()
        {
            string fileToDelete = @"c:\SAHLCommonBusinessModelDAOInMemoryTest.txt";

            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);

            using (TextWriter tw = new StreamWriter(@"c:\SAHLCommonBusinessModelDAOInMemoryTest.txt"))
            {
                var daos = (from type in Assembly.Load("SAHL.Common.BusinessModel.DAO").GetTypes()
                            where type.Name.Contains("_DAO")
                            orderby type.Name
                            select type).ToList<Type>();

                tw.WriteLine("Start Load Save Load - {0}", DateTime.Now);
                System.Diagnostics.Debug.WriteLine("Start Load Save Load - {0}", DateTime.Now);
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                StringBuilder sb = new StringBuilder();

                foreach (var daoType in daos)
                {
                    try
                    {
                        using (new TransactionScope())
                        {
                            tw.WriteLine("testing - {0}", daoType.Name);
                            System.Diagnostics.Debug.WriteLine("testing - {0}", daoType.Name);
                            object daoInstance = Activator.CreateInstance(daoType);
                            SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.LoadSaveLoad(daoInstance, "Key", false);
                            SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                        }
                    }
                    catch (ActiveRecordException aex)
                    {
                        sb.AppendLine(string.Format(daoType.Name));
                        tw.WriteLine("failed - {0}", daoType.Name);
                        tw.WriteLine(aex.Message);
                        tw.WriteLine(aex.StackTrace);

                        System.Diagnostics.Debug.WriteLine("failed - {0}", daoType.Name);
                        System.Diagnostics.Debug.WriteLine(aex.Message);
                        System.Diagnostics.Debug.WriteLine(aex.StackTrace);
                        SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                    }
                    catch (Exception e)
                    {
                        sb.AppendLine(string.Format(daoType.Name));
                        tw.WriteLine("failed - {0}", daoType.Name);
                        tw.WriteLine(e.Message);
                        tw.WriteLine(e.StackTrace);

                        System.Diagnostics.Debug.WriteLine("failed - {0}", daoType.Name);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        System.Diagnostics.Debug.WriteLine(e.StackTrace);
                        SAHL.Common.BusinessModel.DAO.InMemoryTest.DAODataConsistancyChecker.CleanUp();
                    }
                    spc.DomainMessages.Clear();
                }
                tw.WriteLine("End Load Save Load - {0}", DateTime.Now);
                System.Diagnostics.Debug.WriteLine("End Load Save Load - {0}", DateTime.Now);

                tw.WriteLine("---- Failures---------------");
                tw.Write(sb.ToString());
            }
        }

        #endregion FullDAOTestWithLogging

        #region Helpers

        private static void CreateDB()
        {
            if (dbCreated)
                return;

            using (var sessionScope = new SessionScope())
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Start Table Create - {0}", DateTime.Now));

                NHibernateDelegate d = new NHibernateDelegate(ProcessNonQuery);
                IDbCommand cmd = new SAHL.Common.Test.NHibernateSQLiteDriver.SAHLSQLiteCommand();

                foreach (DataRow dr in DT.Rows)
                {
                    string db = dr["db"].ToString();
                    string table = dr["tablename"].ToString();
                    string query = dr["sql"].ToString();
                    cmd.CommandText = query;
                    System.Diagnostics.Trace.WriteLine(string.Format("Creating table {0}", table));
                    switch (db)
                    {
                        case "[2am]":
                            {
                                ActiveRecordMediator.Execute(typeof(AccountSequence_DAO), d, cmd);
                                break;
                            }
                        case "ImageIndex":
                            {
                                ActiveRecordMediator.Execute(typeof(Data_DAO), d, cmd);
                                break;
                            }

                        //case "Warehouse":
                        //    {
                        //        ActiveRecordMediator.Execute(typeof(Audit_DAO), d, cmd);
                        //        break;
                        //    }
                        case "sahldb":
                            {
                                ActiveRecordMediator.Execute(typeof(ClientEmail_DAO), d, cmd);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                System.Diagnostics.Trace.WriteLine(string.Format("End Table Create - {0}", DateTime.Now));
            }
            dbCreated = true;
        }

        private static void GenerateTableScript()
        {
            if (DT != null)
                return;

            using (IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString"))
            {
                string query = @"
                SET NOCOUNT ON
                declare  @AllGeneratedTables table(ID int IDENTITY(1,1) NOT NULL,db varchar(50) NULL,tablename varchar(100) NULL, sql varchar(max) NULL)

                INSERT INTO @AllGeneratedTables
                exec [2AM].[dbo].[p_GenTableCreateScriptForSQLLite]
                INSERT INTO @AllGeneratedTables
                exec ImageIndex.[dbo].[p_GenTableCreateScriptForSQLLite]
                INSERT INTO @AllGeneratedTables
                exec Warehouse.[dbo].[p_GenTableCreateScriptForSQLLite]
                INSERT INTO @AllGeneratedTables
                exec SAHLDB.[dbo].[p_GenTableCreateScriptForSQLLite]
                SELECT * FROM @AllGeneratedTables";

                //                string query = @"
                //                declare  @AllGeneratedTables table(ID int IDENTITY(1,1) NOT NULL,db varchar(50) NULL,tablename varchar(100) NULL, sql varchar(max) NULL)
                //
                //                INSERT INTO @AllGeneratedTables
                //                                SELECT 'sahldb','Regent','CREATE TABLE dbo.Regent ([LoanNumber] INTEGER    NOT NULL ,[RegentClientSalutation] TEXT    NULL ,[RegentClientSurname] TEXT    NULL ,[RegentClientFirstNames] TEXT    NULL ,[RegentClientIDNumber] NUMERIC    NULL ,[RegentClientGender] INTEGER    NULL ,[RegentClientDateBirth] TEXT    NULL ,[RegentPolicyStatus] INTEGER    NULL ,[RegentApplicationDate] TEXT    NULL ,[RegentInceptionDate] TEXT    NULL ,[RegentExpiryDate] TEXT    NULL ,[RegentLoanTerm] NUMERIC    NULL ,[RegentSumInsured] REAL    NULL ,[RegentPremium] REAL    NULL ,[RegentClientSecSalutation] TEXT    NULL ,[RegentClientSecSurname] TEXT    NULL ,[RegentClientSecFirstNames] TEXT    NULL ,[RegentClientSecIDNumber] NUMERIC    NULL ,[RegentClientSecGender] INTEGER    NULL ,[RegentClientSecDateBirth] TEXT    NULL ,[RegentJointIndicator] INTEGER    NULL ,[RegentReinstateDate] TEXT    NULL ,[RegentLastUpdateDate] TEXT    NULL ,[RegentCommision] REAL    NULL ,[RegentUnderwritingFirst] INTEGER    NULL ,[RegentUnderwritingSecond] INTEGER    NULL ,[SAHLEmployeeNumber] NUMERIC    NULL ,[RegentUpdatedStatus] INTEGER    NULL ,[RegentClientOccupation] INTEGER    NULL ,[RegentClientAge] INTEGER    NULL ,[ReplacementPolicy] TEXT    NULL ,[AdviceRequired] TEXT    NULL ,[LifeAssuredName] TEXT    NULL ,[OldInsurer] TEXT    NULL ,[OldPolicyNo] TEXT    NULL ,[RegentNewBusinessDate] TEXT    NULL ,[CapitalizedMonthlyBalance] REAL    NULL )'
                //
                //                select * from @AllGeneratedTables";

                DT = new DataTable();
                Helper.FillFromQuery(DT, query, con, null);
            }
        }

        #endregion Helpers
    }
}