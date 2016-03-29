using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SAHL.Common.Test.NHibernateSQLiteDriver
{
    public class SAHLSQLiteParameter : DbParameter
    {
        private readonly SAHLSQLiteCommand command;
        private readonly SQLiteParameter parameter;

        public SAHLSQLiteParameter(SAHLSQLiteCommand command)
        {
            this.command = command;
            parameter = command.InnerCommand.CreateParameter();
        }

        public override void ResetDbType()
        {
            parameter.ResetDbType();
        }

        public override DbType DbType
        {
            get { return parameter.DbType; }
            set { parameter.DbType = value; }
        }

        public override ParameterDirection Direction
        {
            get { return parameter.Direction; }
            set { parameter.Direction = value; }
        }

        public override bool IsNullable
        {
            get { return parameter.IsNullable; }
            set { parameter.IsNullable = value; }
        }

        public override string ParameterName
        {
            get { return parameter.ParameterName; }
            set { parameter.ParameterName = value; }
        }

        public override string SourceColumn
        {
            get { return parameter.SourceColumn; }
            set { parameter.SourceColumn = value; }
        }

        public override DataRowVersion SourceVersion
        {
            get { return parameter.SourceVersion; }
            set { parameter.SourceVersion = value; }
        }

        public override object Value
        {
            get { return parameter.Value; }
            set { parameter.Value = value; }
        }

        public override int Size
        {
            get { return parameter.Size; }
            set { parameter.Size = value; }
        }

        public override bool SourceColumnNullMapping
        {
            get { return parameter.SourceColumnNullMapping; }
            set { parameter.SourceColumnNullMapping = value; }
        }

        internal SQLiteParameter InnerParameter
        {
            get { return parameter; }
        }
    }
}