using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess
{
    public class FakeBusinessProcessStatement : TaskBaseStatement
    {
        public FakeBusinessProcessStatement()
            : base(null, null, false, null, null, null, null)
        {
        }

        public FakeBusinessProcessStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone,
            string businessProcessName, string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return this.GetType().FullName;
        }
    }
}
