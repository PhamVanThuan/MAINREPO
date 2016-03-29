using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class SAHLSQLiteConnection : DbConnection
    {
        private readonly SQLiteConnection connection;

        public SAHLSQLiteConnection()
        {
            connection = new SQLiteConnection();
        }

        public SAHLSQLiteConnection(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
        }

        public SAHLSQLiteConnection(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new SAHLSQLiteTransaction(this, connection.BeginTransaction(isolationLevel));
        }

        public override void ChangeDatabase(string databaseName)
        {
            connection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            connection.Close();
        }

        public override string ConnectionString
        {
            get { return connection.ConnectionString; }
            set { connection.ConnectionString = value; }
        }

        protected override DbCommand CreateDbCommand()
        {
            return new SAHLSQLiteCommand(this);
        }

        public override string DataSource
        {
            get { return connection.DataSource; }
        }

        public override string Database
        {
            get { return connection.Database; }
        }

        public override void Open()
        {
            connection.Open();
        }

        public override string ServerVersion
        {
            get { return connection.ServerVersion; }
        }

        public override ConnectionState State
        {
            get { return connection.State; }
        }

        protected override void Dispose(bool disposing)
        {
            connection.Dispose();
            base.Dispose(disposing);
        }

        internal SQLiteConnection InnerConnection
        {
            get { return connection; }
        }
    }
}