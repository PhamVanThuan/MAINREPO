using System;
using System.Collections.Generic;
using SAHL.Core.Data;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements
{
    public interface ITaskStatementSelector
    {
        string CreateLookupKey(string typeFullName);
        IEnumerable<Type> GetTaskStatementTypes();
        Type GetStatementTypeForWorkFlow(string businessProcessName, string workFlowName, string stateName);
    }
}