using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace Database.Tests.Base
{
    public class TestProvider
    {
        public static IEnumerable<string> GetAllTests()
        {
            List<string> tests = new List<string>();

            tests.AddRange(GetAllProcessTests());
            tests.AddRange(GetAll2AMTests());

            return tests;
        }

        public static IEnumerable<string> GetAllProcessTests()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForDB("Process_Test");
            }
        }

        public static IEnumerable<string> GetAll2AMTests()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForDB("[2AM_Test]");
            }
        }

        public static IEnumerable<string> GetTests_backend()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "backend");
            }
        }

        public static IEnumerable<string> GetTests_batch()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "batch");
            }
        }

        public static IEnumerable<string> GetTests_dbo()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "dbo");
            }
        }

        public static IEnumerable<string> GetTests_deb()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "deb");
            }
        }

        public static IEnumerable<string> GetTests_fin()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "fin");
            }
        }

        public static IEnumerable<string> GetTests_fincore()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "fincore");
            }
        }

        public static IEnumerable<string> GetTests_frameworktest()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "frameworktest");
            }
        }

        public static IEnumerable<string> GetTests_halo()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "halo");
            }
        }

        public static IEnumerable<string> GetTests_halonotran()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "halonotran");
            }
        }

        public static IEnumerable<string> GetTests_hoc()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "hoc");
            }
        }

        public static IEnumerable<string> GetTests_monthend()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "monthend");
            }
        }

        public static IEnumerable<string> GetTests_origination()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "origination");
            }
        }

        public static IEnumerable<string> GetTests_paymentallocation()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "paymentallocation");
            }
        }

        public static IEnumerable<string> GetTests_personalloan()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "pl");
            }
        }

        public static IEnumerable<string> GetTests_product()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "product");
            }
        }

        public static IEnumerable<string> GetTests_report()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "report");
            }
        }

        public static IEnumerable<string> GetTests_security()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "security");
            }
        }

        public static IEnumerable<string> GetTests_spv()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "spv");
            }
        }

        public static IEnumerable<string> GetTests_spvdet()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "spvdet");
            }
        }

        public static IEnumerable<string> GetTests_spvrh()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "spvrh");
            }
        }

        public static IEnumerable<string> GetTests_tranpost()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "tranpost");
            }
        }

        public static IEnumerable<string> GetTests_trigger()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsForSchema("Process_Test", "trigger");
            }
        }

        public static IEnumerable<string> GetIgnoredTests()
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return GetTestsBySchema("Process_Test", "ignored_tests");
            }
        }

        /// <summary>
        /// Gets all the tests from a test db
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetTestsForDB(string dbName)
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return db.List<string>(
                    string.Format(@"select pr.name
                                    from {0}.sys.procedures pr
                                    join {0}.sys.schemas s ON pr.schema_id = s.schema_id
	                                    and s.name != 'ignored_tests'
                                    where pr.name like 'sqltest_%'
                                    and pr.[type] = 'P'
                                    order by pr.name", dbName));
            }
        }

        /// <summary>
        /// Gets all the tests from a test db and for the schema of the proc being tested i.e. not the schema of the test proc
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetTestsForSchema(string dbName, string schemaName)
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return db.List<string>(
                                        string.Format(@"select pr.name
                                    from {0}.sys.procedures pr
                                    join {0}.sys.schemas s ON pr.schema_id = s.schema_id
	                                    and s.name != 'ignored_tests'
                                    where pr.name like 'sqltest_{1}#%'
                                    and pr.[type] = 'P'
                                    order by pr.name", dbName, schemaName.ToLower()));
            }
        }

        /// <summary>
        /// Gets all the tests from a test db and for the schema of the TEST proc
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetTestsBySchema(string dbName, string schemaName)
        {
            using (IDbConnection db = DB.Factory.OpenDbConnection())
            {
                return db.List<string>(
                                        string.Format(@"select pr.name
                                    from {0}.sys.procedures pr
                                    join {0}.sys.schemas s ON pr.schema_id = s.schema_id
	                                    and s.name = '{1}'
                                    where pr.name like 'sqltest_%'
                                    and pr.[type] = 'P'
                                    order by pr.name", dbName, schemaName.ToLower()));
            }
        }
    }
}