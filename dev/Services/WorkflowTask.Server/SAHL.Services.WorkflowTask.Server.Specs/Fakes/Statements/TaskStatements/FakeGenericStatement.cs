using SAHL.Core.Data;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements
{
    public class FakeGenericStatement : TaskBaseStatement, ISqlStatement<object>, IGenericStatement
    {
        public FakeGenericStatement()
            : base(null, null, false, null, null, null, null)
        {
        }

        public FakeGenericStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName, 
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
