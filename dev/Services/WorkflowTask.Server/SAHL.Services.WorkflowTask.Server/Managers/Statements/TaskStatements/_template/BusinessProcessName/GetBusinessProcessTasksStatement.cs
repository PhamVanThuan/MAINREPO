using System;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements._template.BusinessProcessName
{
    /// <summary>
    ///     Template class for the statement that will execute when no specific workflow is found for a given task
    /// </summary>
    internal class GetBusinessProcessTasksStatement : TaskBaseStatement
    {
        public GetBusinessProcessTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone,
            string businessProcessName, string workFlowName, List<string> workFlowStateNames)
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
