using SAHL.Core.Data;

namespace SAHL.X2Engine2.ViewModels.SqlStatement
{
    public class InstanceSqlStatement : ISqlStatement<_Instance>
    {
        public long InstanceId { get; protected set; }

        public InstanceSqlStatement(long instanceId)
        {
            this.InstanceId = instanceId;
        }

        public string GetStatement()
        {
            return @"select i.ID as InstanceId, i.StateId as StateId, s.Name as StateName, i.StateChangeDate as StateChangeDate, w.Name as WorkflowName
                    , p.Name as ProcessName, i.CreatorAdUserName as CreatorADUsername
                    from x2.x2.instance i
                    join x2.x2.workflow w on i.workflowid=w.id
                    join x2.x2.process p on w.processid=p.id
                    left join x2.x2.state s on i.StateId=s.id
                    where i.id=@InstanceId";
        }
    }
}