using SAHL.Core.Data;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class ActivityByNameAndWorkflowNameSqlStatement : ISqlStatement<Activity>
    {
        public ActivityByNameAndWorkflowNameSqlStatement(string activityName, string workflowName)
        {
            this.ActivityName = activityName;
            this.WorkflowName = workflowName;
        }

        public string ActivityName { get; protected set; }

        public string WorkflowName { get; protected set; }

        public string GetStatement()
        {
            string sql = @"select top 1 a.ID as ActivityID, a.name as ActivityName, a.StateId as StateId, IsNULL(sFrom.Name,'') as StateName, s2.ID as ToStateId, s2.Name as ToStateName,
                    w.Id as WorkflowId, a.SplitWorkflow as SplitWorkflow, a.RaiseExternalActivity as RaiseExternalActivityId, a.Priority as Priority
                    from x2.x2.Activity a
                    join x2.x2.workflow w on a.WorkflowID=w.ID
                    join x2.x2.state s2 on a.NextStateId=s2.ID
                    left join x2.x2.State sFrom on a.StateID=sFrom.ID
                    where w.Name=@WorkflowName and a.Name=@ActivityName
                    order by w.id desc;";
            return sql;
        }
    }
}