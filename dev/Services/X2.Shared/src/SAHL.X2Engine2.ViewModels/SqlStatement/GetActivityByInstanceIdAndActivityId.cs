using SAHL.Core.Data;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class GetActivityByInstanceIdAndActivityId : ISqlStatement<Activity>
    {
        public long InstanceId { get; protected set; }

        public string ActivityName { get; protected set; }

        public GetActivityByInstanceIdAndActivityId(long instanceId, string activityName)
        {
            this.ActivityName = activityName;
            this.InstanceId = instanceId;
        }

        public string GetStatement()
        {
            string sql = @"select top 1 a.ID as ActivityID, a.Name as ActivityName,
case when a.Type=10 then (select StateID from x2.x2.instance where ID=@InstanceId) else i.stateId end as StateId,
case when a.Type=10 then (select s.Name from x2.x2.instance i join x2.x2.state s on i.stateid=s.id where i.ID=@InstanceId) else IsNULL(s.name,'') end as StateName, a.NextStateID as ToStateId,
sTo.Name as ToStateName, i.workflowID as WorkflowId, a.SplitWorkflow as SplitWorkflow, a.RaiseExternalActivity as RaiseExternalActivityID,
a.Priority as Priority
from x2.x2.instance i
join x2.x2.activity a on i.workflowId=a.workflowId
join x2.x2.state sTo on a.NextStateID=sTo.ID
left join x2.x2.state s on i.stateid=s.id
where a.Name=@ActivityName and i.id=@InstanceId
order by 1 desc";
            return sql;
        }
    }
}