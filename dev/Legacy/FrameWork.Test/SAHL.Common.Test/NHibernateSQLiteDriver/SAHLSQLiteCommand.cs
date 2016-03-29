using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class SAHLSQLiteCommand : DbCommand
    {
        private readonly SQLiteCommand command;
        private readonly SAHLSQLiteParameterCollection parameters;
        private SAHLSQLiteConnection connection;
        private SAHLSQLiteTransaction transaction;

        public SAHLSQLiteCommand()
            : this(new SAHLSQLiteConnection())
        {
        }

        public SAHLSQLiteCommand(SAHLSQLiteConnection connection)
        {
            this.connection = connection;
            command = connection.InnerConnection.CreateCommand();
            parameters = new SAHLSQLiteParameterCollection(command.Parameters);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (command != null)
            {
                command.Dispose();
            }
            //command.Dispose();
            /*
            if (transaction != null)
            {
                transaction.Dispose();
            }
            */
        }

        internal SQLiteCommand InnerCommand
        {
            get { return command; }
        }

        public override void Cancel()
        {
            command.Cancel();
        }

        public override string CommandText
        {
            get { return command.CommandText; }
            set { command.CommandText = new CommandTextParser().ParseCommandText(value); }
        }

        public override int CommandTimeout
        {
            get { return command.CommandTimeout; }
            set { command.CommandTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return command.CommandType; }
            set { command.CommandType = value; }
        }

        protected override DbParameter CreateDbParameter()
        {
            return new SAHLSQLiteParameter(this);
        }

        protected override DbConnection DbConnection
        {
            get { return connection; }
            set
            {
                connection = (SAHLSQLiteConnection)value;
                command.Connection = connection.InnerConnection;
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get { return transaction; }
            set
            {
                if (value == null)
                    return;
                SAHLSQLiteTransaction tran = value as SAHLSQLiteTransaction;
                transaction = (SAHLSQLiteTransaction)value;
                Connection = transaction.Connection;
                command.Transaction = transaction.InnerTransaction;
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return command.ExecuteReader(behavior);
        }

        public override int ExecuteNonQuery()
        {
            return command.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return command.ExecuteScalar();
        }

        public override void Prepare()
        {
            command.Prepare();
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get { return command.UpdatedRowSource; }
            set { command.UpdatedRowSource = value; }
        }
    }
}