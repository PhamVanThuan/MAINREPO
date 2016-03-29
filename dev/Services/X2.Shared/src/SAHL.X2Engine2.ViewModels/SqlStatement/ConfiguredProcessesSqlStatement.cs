using SAHL.Core.Data;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ConfiguredProcessesSqlStatement : ISqlStatement<ProcessViewModel>
    {
        public string InClause { get; set; }

        public ConfiguredProcessesSqlStatement(string inClause)
        {
            this.InClause = inClause;
        }

        public string GetStatement()
        {
            string sql = string.Format("select max(id) as processId, Name as processName from x2.X2.Process where Name in ({0}) group by Name order by Name", InClause);
            return sql;
        }
    }
}