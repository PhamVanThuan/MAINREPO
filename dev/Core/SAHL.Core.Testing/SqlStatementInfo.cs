using System.Collections.Generic;

namespace SAHL.Core.Testing
{
    public sealed class SqlStatementInfo
    {

        private string sql;

        public SqlStatementInfo(string sql)
        {
            this.sql = sql.ToLower();
        }

        public SqlStatementType StatementType
        {
            get
            {
                if (IsInsertStatement())
                {
                    return SqlStatementType.Insert;
                }
                
                if (IsDeleteStatement())
                {
                    return SqlStatementType.Delete;
                }
                
                if (IsSelectStatement())
                {
                    return SqlStatementType.Select;
                }

                if (IsUpdateStatement())
                {
                    return SqlStatementType.Update;
                }

                return SqlStatementType.None;
            }
        }

        private bool IsUpdateStatement()
        {
            return this.sql.StartsWith("update") || sql.Contains("update ");
        }

        private bool IsSelectStatement()
        {
            return this.sql.StartsWith("select") || this.sql.StartsWith("declare") || this.sql.StartsWith("with") || this.sql.StartsWith("execute");
        }

        private bool IsDeleteStatement()
        {
            return this.sql.Contains("delete from");
        }

        private bool IsInsertStatement()
        {
            return this.sql.StartsWith("insert");
        }
    }
}
