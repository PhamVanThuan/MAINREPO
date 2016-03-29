using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.Globals;
using SAHL.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SAHL.Common.Test.DataAccess
{
    /// <summary>
    /// Tests the DBHelper class.
    /// </summary>
    [TestFixture]
    public class DBHelperTest
    {

        private string ConnectionString = null;

        [SetUp]
        public void SetUp()
        {
            if (String.IsNullOrEmpty(ConnectionString))
            {
                foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
                {
                    if (css.ConnectionString.ToLower().IndexOf("2am") > -1)
                        ConnectionString = css.ConnectionString;
                }
            }

            if (String.IsNullOrEmpty(ConnectionString))
                throw new Exception("Unable to find 2am connection in config file");
        }


        [Test]
        public void ConstructorTest()
        {
            // test each of the enumeration values
            foreach (int db in Enum.GetValues(typeof(Databases)))
            {
                using (DBHelper dbHelper = new DBHelper((Databases)db))
                {
                    Assert.IsNotNull(dbHelper.Connection);
                    Assert.AreEqual(dbHelper.Connection.State, ConnectionState.Open);
                }
            }

            // test using a connection string
            using (DBHelper dbHelper = new DBHelper(ConnectionString))
            {
                Assert.IsNotNull(dbHelper.Connection);
                Assert.AreEqual(dbHelper.Connection.State, ConnectionState.Open);
            }

        }

        /// <summary>
        /// Tests the DBHelper.CreateCommand method.
        /// </summary>
        [Test]
        public void CreateCommand()
        {
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                string commandText = "select * from Nothing";
                ParameterCollection parameters = new ParameterCollection();
                parameters.Add(new SqlParameter());

                // CreateCommand()
                IDbCommand command = dbHelper.CreateCommand();
                Assert.IsNotNull(command);
                command.Dispose();

                // CreateCommand(string)
                command = dbHelper.CreateCommand(commandText);
                Assert.IsNotNull(command);
                Assert.AreEqual(command.CommandText, commandText);
                command.Dispose();

                // CreateCommand(string, ParameterCollection)
                command = dbHelper.CreateCommand(commandText, parameters);
                Assert.IsNotNull(command);
                Assert.AreEqual(command.CommandText, commandText);
                Assert.AreEqual(command.Parameters.Count, parameters.Count);
                command.Dispose();
            }
        }

        /// <summary>
        /// Tests the DBHelper.CreateConnection method.
        /// </summary>
        [Test]
        public void CreateConnection()
        {
            // create a connection that is initialised as open
            using (DBHelper dbHelper = new DBHelper())
            {
                Assert.IsNull(dbHelper.Connection);
                dbHelper.CreateConnection(ConnectionString, true);
                Assert.IsNotNull(dbHelper.Connection);
                Assert.AreEqual(dbHelper.Connection.State, ConnectionState.Open);
            }

            // create a connection that is initialised as closed
            using (DBHelper dbHelper = new DBHelper())
            {
                Assert.IsNull(dbHelper.Connection);
                dbHelper.CreateConnection(ConnectionString, false);
                Assert.IsNotNull(dbHelper.Connection);
                Assert.AreEqual(dbHelper.Connection.State, ConnectionState.Closed);
            }
        }

        /// <summary>
        /// Tests the DBHelper.ExecuteNonQuery method.
        /// </summary>
        [Test]
        public void ExecuteNonQuery()
        {

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                if (dbHelper.Connection == null)
                    Assert.Ignore("Unable to establish connection");

                string sql1 = "SELECT top 1 AccountKey from [2am]..Account (nolock)";
                string sql2 = "SELECT top 1 AccountKey from [2am]..Account (nolock) where AccountKey = @AccountKey";

                // get an account key to work with
                int accountKey = Convert.ToInt32(dbHelper.ExecuteScalar(sql1));

                // execute simple statement
                dbHelper.ExecuteNonQuery(sql1);

                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@AccountKey", accountKey));

                // test with parameters
                dbHelper.ExecuteNonQuery(sql2, pc);

                // test using a command
                IDbCommand cmd = dbHelper.CreateCommand(sql2, pc);
                dbHelper.ExecuteNonQuery(cmd);

            }
        }
        
        /// <summary>
        /// Tests the DBHelper.ExecuteReader method.
        /// </summary>
        [Test]
        public void ExecuteReader()
        {

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                if (dbHelper.Connection == null)
                    Assert.Ignore("Unable to establish connection");

                string sql1 = "SELECT top 1 AccountKey from [2am]..Account (nolock)";
                string sql2 = "SELECT top 1 AccountKey from [2am]..Account (nolock) where AccountKey = @AccountKey";
                int accountKey;

                // test simple execution with no parameters
                using (IDataReader reader = dbHelper.ExecuteReader(sql1))
                {
                    reader.Read();
                    accountKey = reader.GetInt32(0);
                }

                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@AccountKey", accountKey));

                // test with parameters
                using (IDataReader reader = dbHelper.ExecuteReader(sql2, pc))
                {
                    reader.Read();
                    int a = reader.GetInt32(0);
                    Assert.AreEqual(accountKey, a);
                }

                // test using a command
                IDbCommand cmd = dbHelper.CreateCommand(sql2, pc);
                using (IDataReader reader = dbHelper.ExecuteReader(cmd))
                {
                    reader.Read();
                    int a = reader.GetInt32(0);
                    Assert.AreEqual(accountKey, a);
                }

            }
        }

        /// <summary>
        /// Tests the DBHelper.ExecuteScalar method.
        /// </summary>
        [Test]
        public void ExecuteScalar()
        {

            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                if (dbHelper.Connection == null)
                    Assert.Ignore("Unable to establish connection");

                string sql1 = "SELECT top 1 AccountKey from [2am]..Account (nolock)";
                string sql2 = "SELECT top 1 AccountKey from [2am]..Account (nolock) where AccountKey = @AccountKey";

                // test simple execution with no parameters
                int accountKey = Convert.ToInt32(dbHelper.ExecuteScalar(sql1));

                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@AccountKey", accountKey));

                // test with parameters
                int a = Convert.ToInt32(dbHelper.ExecuteScalar(sql2, pc));
                Assert.AreEqual(accountKey, a);

                // test using a command
                IDbCommand cmd = dbHelper.CreateCommand(sql2, pc);
                a = Convert.ToInt32(dbHelper.ExecuteScalar(cmd));
                Assert.AreEqual(accountKey, a);

            }
        }

        [Test]
        public void Fill()
        {
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                if (dbHelper.Connection == null)
                    Assert.Ignore("Unable to establish connection");

                string sql = "select count(*) from GeneralStatus";
                int count = Convert.ToInt32(dbHelper.ExecuteScalar(sql));

                if (count == 0)
                    Assert.Ignore("No data");

                sql = "select * from GeneralStatus";

                // test the fill with command object
                IDbCommand cmd = dbHelper.CreateCommand(sql);
                DataTable dt = new DataTable();
                dbHelper.Fill(dt, cmd);
                Assert.AreEqual(dt.Rows.Count, count);
                dt.Dispose();
                cmd.Dispose();

                // test the fill with AutoDispose of the command
                dt = new DataTable();
                dbHelper.Fill(dt, dbHelper.CreateCommand(sql), true);
                Assert.AreEqual(dt.Rows.Count, count);
                dt.Dispose();

                // test the fill with a simple query
                dt = new DataTable();
                dbHelper.Fill(dt, sql);
                Assert.AreEqual(dt.Rows.Count, count);
                dt.Dispose();

                // test the fill with a query and parameters
                sql += " where GeneralStatusKey = " + ((int)GeneralStatuses.Active).ToString();
                dt = new DataTable();
                dbHelper.Fill(dt, sql);
                Assert.AreEqual(dt.Rows.Count, 1);
                dt.Dispose();
            }
        }


    }
}
