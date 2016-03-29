using SAHL.Core.Data;
using SAHL.Core.Data;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Tests.X2.Models;
namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public class X2WorkflowStates : ISqlStatement<X2State>
    {
        public int WorkflowId { get; protected set; }
        public X2WorkflowStates(int WorkflowId)
        {
            this.WorkflowId = WorkflowId;
        }
        public string GetStatement()
        {
            return @"select s.Name x2.x2.state as s where s.workflowid = @WorkflowId";
        }
    }
}
