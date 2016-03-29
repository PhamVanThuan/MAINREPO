using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class SAHLSQLiteTransaction : DbTransaction
    {
        private readonly SAHLSQLiteConnection connection;
        private readonly SQLiteTransaction transaction;

        public SAHLSQLiteTransaction(SAHLSQLiteConnection connection, SQLiteTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        public override void Commit()
        {
            transaction.Commit();
        }

        protected override DbConnection DbConnection
        {
            get { return connection; }
        }

        public override IsolationLevel IsolationLevel
        {
            get { return transaction.IsolationLevel; }
        }

        public override void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }

        internal SQLiteTransaction InnerTransaction
        {
            get { return transaction; }
        }
    }
}