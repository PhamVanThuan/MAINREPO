using SAHL.Core.Data;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Tests.X2.Models;
namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public class X2WorkflowsSqlStatement: ISqlStatement<X2ProcessWorkflow>
    {
        public string GetStatement()
        {
            return @"select 
                        p.Name as Process,
                        w.name as Workflow,
                        w.Id as WorkflowId
                     from X2.X2.Workflow w
                        join X2.X2.Process p 
                            on w.ProcessID = p.ID
                     order by w.ID desc";
        }
    }
}
