using System.Configuration;
using ServiceStack.OrmLite;

namespace Database.Tests
{
    public static class DB
    {
        private static string testDB_ConnectionString;

        public static string TestDB_ConnectionString
        {
            get
            {
                if (testDB_ConnectionString == null)
                {
                    testDB_ConnectionString = ConfigurationManager.ConnectionStrings["Database.Tests.Base.Settings.TestDB"].ConnectionString;
                }
                return testDB_ConnectionString;
            }
        }

        private static OrmLiteConnectionFactory factory;

        public static OrmLiteConnectionFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new OrmLiteConnectionFactory(TestDB_ConnectionString, SqlServerDialect.Provider);
                }
                return factory;
            }
        }
    }
}