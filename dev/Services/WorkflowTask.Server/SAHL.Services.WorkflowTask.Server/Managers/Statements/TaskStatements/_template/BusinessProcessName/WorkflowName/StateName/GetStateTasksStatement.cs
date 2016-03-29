using System;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements._template.BusinessProcessName.WorkflowName.StateName
{
    /// <summary>
    /// Template class for the statement that will execute when a specific business process, workflow and state are all found
    /// </summary>
    class GetStateTasksStatement : TaskBaseStatement
    {
        public GetStateTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName, string workFlowName,
                                        List<string> workFlowStateNames)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, null)
        {
        }

        public override string GetStatement()
        {
            //Will not be implemented, this is a template illustrating the structure
            throw new NotImplementedException();
        }
    }
}
