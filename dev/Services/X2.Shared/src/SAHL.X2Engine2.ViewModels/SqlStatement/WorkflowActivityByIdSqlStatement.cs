using SAHL.Core.Data;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class WorkflowActivityByIdSqlStatement : ISqlStatement<WorkflowActivity>
    {
        public int Id { get; protected set; }

        public WorkflowActivityByIdSqlStatement(int Id)
        {
            this.Id = Id;
        }

        public string GetStatement()
        {
            string sql = @"select ID, WorkFlowID, Name, NextWorkFlowID, NextActivityID, StateID, ReturnActivityID
                            from x2.X2.WorkFlowActivity (nolock)
                            where ID=@Id";
            return sql;
        }
    }
}