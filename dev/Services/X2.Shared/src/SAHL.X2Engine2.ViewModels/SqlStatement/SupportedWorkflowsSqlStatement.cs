using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class SupportedWorkflowsSqlStatement : ISqlStatement<ProcessWorkflowViewModel>
    {
        public string InClause { get; set; }

        public SupportedWorkflowsSqlStatement(string inClause)
        {
            this.InClause = inClause;
        }
        public string GetStatement()
        {
            string sql = string.Format(@";with processes as
                                        (select max(id) as processId, Name as processName from x2.X2.Process where Name in ({0}) group by Name)
                                        select wf.id as workflowid, wf.name as workflowName, p.processId, p.processName
                                        from processes p
                                        join x2.x2.workflow wf
                                        on wf.ProcessID = p.ProcessID", InClause);
            return sql;
        }
    }
}
