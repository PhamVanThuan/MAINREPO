using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public partial class DapperUIStatements
    {
        public DapperUIStatements(BusinessModelDescription businessModelDescription)
        {
            this.BusinessModelDescription = businessModelDescription;
        }

        public BusinessModelDescription BusinessModelDescription { get; protected set; }

        public string GetSelectList()
        {
            StringBuilder sb = new StringBuilder();
            int iteratorCount = 0;
            int totalCount = this.BusinessModelDescription.Properties.Count();
            foreach (var property in this.BusinessModelDescription.Properties)
            {
                sb.Append(string.Format("{0}", this.EscapeIfReservedWord(property.PropertyName)));
                iteratorCount++;
                if (iteratorCount < totalCount)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        public string GetInsertList()
        {
            StringBuilder sb = new StringBuilder();
            int iteratorCount = 0;
            int totalCount = this.BusinessModelDescription.Properties.Count();
            foreach (var property in this.BusinessModelDescription.Properties)
            {
                iteratorCount++;
                if (!property.IsIdentitySeed && !property.IsComputed)
                {
                    sb.Append(string.Format("{0}", this.EscapeIfReservedWord(property.PropertyName)));
                    if (iteratorCount < totalCount)
                    {
                        sb.Append(", ");
                    }
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 2, 2) == ", ")
            {
                sb.Remove(sb.ToString().Length - 2, 2);
            }
            return sb.ToString();
        }

        public string GetInsertValuesList()
        {
            StringBuilder sb = new StringBuilder();
            int iteratorCount = 0;
            int totalCount = this.BusinessModelDescription.Properties.Count();
            foreach (var property in this.BusinessModelDescription.Properties.OrderByDescending(x=>x.IsPrimaryKey))
            {
                if (!property.IsIdentitySeed && !property.IsComputed)
                {
                    sb.Append(string.Format("@{0}", property.PropertyName));
                    iteratorCount++;
                    if (iteratorCount < totalCount)
                    {
                        sb.Append(", ");
                    }
                }
                else
                {
                    iteratorCount++;
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 2, 2) == ", ")
            {
                sb.Remove(sb.ToString().Length-2,2);
            }

            return sb.ToString();
        }

        public string GetUpdateList()
        {
            StringBuilder sb = new StringBuilder();
            int iteratorCount = 0;
            int totalCount = this.BusinessModelDescription.Properties.Count();
            foreach (var property in this.BusinessModelDescription.Properties.OrderByDescending(x => x.IsPrimaryKey))
            {
                if (!property.IsIdentitySeed && !property.IsComputed)
                {
                    sb.Append(string.Format("{0} = @{1}", this.EscapeIfReservedWord(property.PropertyName), property.PropertyName));
                    iteratorCount++;
                    if (iteratorCount < totalCount)
                    {
                        sb.Append(", ");
                    }
                }
                else
                {
                    iteratorCount++;
                }
            }
            if (sb.ToString().Substring(sb.ToString().Length - 2, 2) == ", ")
            {
                sb.Remove(sb.ToString().Length - 2, 2);
            }

            return sb.ToString();
        }

        public string EscapeIfReservedWord(string identifier)
        {
            List<string> reservedWords = new List<string>(new string[]{"ADD", "ALL", "ALTER", "AND", "ANY", "AS", "ASC", "AUTHORIZATION", "BACKUP", "BEGIN", "BETWEEN", "BREAK", "BROWSE", "BULK",
                                                                       "BY", "CASCADE", "CASE", "CHECK", "CHECKPOINT", "CLOSE", "CLUSTERED", "COALESCE", "COLLATE", "COLUMN", "COMMIT", "COMPUTE",
                                                                       "CONSTRAINT", "CONTAINS", "CONTAINSTABLE", "CONTINUE", "CONVERT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME",
                                                                       "CURRENT_TIMESTAMP", "CURRENT_USER", "CURSOR", "DATABASE", "DBCC", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DENY", "DESC",
                                                                       "DISK", "DISTINCT", "DISTRIBUTED", "DOUBLE", "DROP", "DUMP", "ELSE", "END", "ERRLVL", "ESCAPE", "EXCEPT", "EXEC", "EXECUTE", "EXISTS",
                                                                       "EXIT",
"EXTERNAL",
"FETCH",
"FILE",
"FILLFACTOR",
"FOR",
"FOREIGN",
"FREETEXT",
"FREETEXTTABLE",
"FROM",
"FULL",
"FUNCTION",
"GOTO",
"GRANT",
"GROUP",
"HAVING",
"HOLDLOCK",
"IDENTITY",
"IDENTITY_INSERT",
"IDENTITYCOL",
"IF",
"IN",
"INDEX",
"INNER",
"INSERT",
"INTERSECT",
"INTO",
"IS",
"JOIN",
"KEY",
"KILL",
"LEFT",
"LIKE",
"LINENO",
"LOAD",
"MERGE",
"NATIONAL",
"NOCHECK",
"NONCLUSTERED",
"NOT",
"NULL",
"NULLIF",
"OF",
"OFF",
"OFFSETS",
"ON",
"OPEN",
"OPENDATASOURCE",
"OPENQUERY",
"OPENROWSET",
"OPENXML",
"OPTION",
"OR",
"ORDER",
"OUTER",
"OVER",
"PERCENT",
"PIVOT",
"PLAN",
"PRECISION",
"PRIMARY",
"PRINT",
"PROC",
"PROCEDURE",
"PUBLIC",
"RAISERROR",
"READ",
"READTEXT",
"RECONFIGURE",
"REFERENCES",
"REPLICATION",
"RESTORE",
"RESTRICT",
"RETURN",
"REVERT",
"REVOKE",
"RIGHT",
"ROLLBACK",
"ROWCOUNT",
"ROWGUIDCOL",
"RULE",
"SAVE",
"SCHEMA",
"SECURITYAUDIT",
"SELECT",
"SEMANTICKEYPHRASETABLE",
"SEMANTICSIMILARITYDETAILSTABLE",
"SEMANTICSIMILARITYTABLE",
"SESSION_USER",
"SET",
"SETUSER",
"SHUTDOWN",
"SOME",
"STATISTICS",
"SYSTEM_USER",
"TABLE",
"TABLESAMPLE",
"TEXTSIZE",
"THEN",
"TO",
"TOP",
"TRAN",
"TRANSACTION",
"TRIGGER",
"TRUNCATE",
"TRY_CONVERT",
"TSEQUAL",
"UNION",
"UNIQUE",
"UNPIVOT",
"UPDATE",
"UPDATETEXT",
"USE",
"USER",
"VALUES",
"VARYING",
"VIEW",
"WAITFOR",
"WHEN",
"WHERE",
"WHILE",
"WITH",
"WITHIN GROUP",
"WRITETEXT"});

            if (reservedWords.Contains(identifier.ToUpper()))
            {
                return string.Format("[{0}]", identifier);
            }
            else
            {
                return identifier;
            }
        }
    }
}