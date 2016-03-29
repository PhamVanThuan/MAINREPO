using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class StateByAutoForwardAndWorkflowNameSqlStatement : ISqlStatement<StateDataModel>
    {
        public string WorkflowName { get; set; }

        public string AutoForwardStateName { get; set; }

        public StateByAutoForwardAndWorkflowNameSqlStatement(string autoForwardStateName, string workflowName)
        {
            this.WorkflowName = workflowName;
            this.AutoForwardStateName = autoForwardStateName;
        }

        public string GetStatement()
        {
            string sql = @"select top 1 s.iD, s.workFlowID, s.name, s.type, s.forwardState, s.sequence, s.returnWorkflowID, s.returnActivityID, s.x2ID
                        from x2.x2.state s
                        join x2.x2.workflow w on s.workflowID=w.id
                        where s.Name=@AutoForwardStateName and w.Name=@WorkflowName
                        order by 1 desc";
            return sql;
        }
    }
}