using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess.FakeWorkflow.FakeState
{
    public class FakeStateStatement : TaskBaseStatement
    {
        public FakeStateStatement()
            : base(null, null, false, null, null, null, null)
        {
        }

        public FakeStateStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName, 
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return this.GetType().FullName;
        }
    }
}
