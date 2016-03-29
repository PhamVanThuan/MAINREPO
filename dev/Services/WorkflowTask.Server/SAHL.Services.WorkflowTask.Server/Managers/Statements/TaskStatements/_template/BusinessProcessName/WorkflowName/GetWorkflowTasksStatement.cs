using System;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements._template.BusinessProcessName.WorkflowName
{
    /// <summary>
    /// Template class for the statement that will execute when no specific state is found for a given task
    /// </summary>
    class GetWorkflowTasksStatement : TaskBaseStatement
    {
        public GetWorkflowTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName, string workFlowName,
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
