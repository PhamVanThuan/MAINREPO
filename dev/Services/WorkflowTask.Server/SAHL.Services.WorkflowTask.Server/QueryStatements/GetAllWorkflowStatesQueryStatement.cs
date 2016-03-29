using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.WorkflowTask.Models;
using SAHL.Services.Interfaces.WorkflowTask.Queries;

namespace SAHL.Services.WorkflowTask.Server.QueryStatements
{
    [NolockConventionExclude]
    public class GetAllWorkflowStatesQueryStatement : IServiceQuerySqlStatement<GetAllWorkflowStatesQuery, GetAllWorkflowStatesQueryResult>
    {
        public string GetStatement()
        {
            return @"
select distinct w.name as Workflow--, s.name State
from x2.x2.activity a (nolock)
join x2.x2.workflow w (nolock) on w.id=a.workflowid
join x2.x2.state s (nolock) on (s.id = a.StateID or s.id = a.NextStateID) and s.Type in (1,5)
where a.WorkFlowID in (
    select distinct ww.id 
    from x2.x2.workflow ww (nolock)
    join x2.x2.process p (nolock) on p.id=w.ProcessID 
    where p.id in (
        select max(id)
        from x2.x2.Process (nolock)
        group by name)
    )
and w.name not in (
    'Create Instance V3', 
    'Delete Debit Order', 
    'InterestOnlySMS', 
    'RCS', 
    'IT', 
    'TestMap',
    'Loan Adjustments', 
    'Release And Variations',
    'Quick Cash'
)
and a.splitworkflow=0
order by 1
";
        }
    }
}
