using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class WorkflowActivitiesForStateSqlStatement : ISqlStatement<WorkFlowActivityDataModel>
    {
        public int StateId { get; protected set; }

        public WorkflowActivitiesForStateSqlStatement(int stateId)
        {
            this.StateId = stateId;
        }

        public string GetStatement()
        {
            string sql = @"select ID, WorkFlowID, Name, NextWorkFlowID, NextActivityID, StateID, ReturnActivityID
                            from x2.X2.WorkFlowActivity (nolock)
                            where StateID=@StateID";
            return sql;
        }
    }
}