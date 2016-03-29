using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server
{
    public class TaskQueryResultCollator : ITaskQueryResultCollator
    {
        private TaskBaseStatement taskStatement;

        public TaskQueryResultCollator(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, string stateName, Type taskStatementType, string storageTable)
        {
            this.BusinessProcessName = businessProcessName;
            this.WorkFlowName = workFlowName;
            this.StateName = stateName;
            this.TaskStatementType = taskStatementType;
            this.Username = username;
            this.Roles = roles;
            this.IncludeTasksWithTheRoleOfEveryone = includeTasksWithTheRoleOfEveryone;
            this.StorageTable = storageTable;
        }

        public string BusinessProcessName { get; private set; }
        public string WorkFlowName { get; private set; }
        public string StateName { get; private set; }
        public string StorageTable { get; private set; }
        public Type TaskStatementType { get; private set; }

        public string Username { get; private set; }
        public List<string> Roles { get; private set; }
        public bool IncludeTasksWithTheRoleOfEveryone { get; private set; }

        public IEnumerable<IDictionary<string, object>> Results { get; set; }

        public TaskBaseStatement CreateTaskStatement()
        {
            if (this.taskStatement == null)
            {
                this.taskStatement = ConstructTaskStatement();
            }
            return this.taskStatement;
        }

        private TaskBaseStatement ConstructTaskStatement()
        {
            var parameters = new object[]
            {
                this.Username, //string username
                this.Roles, //List<string> roles
                false, //bool includeTasksWithTheRoleOfEveryone
                this.BusinessProcessName, //string businessProcessName
                this.WorkFlowName, //string workFlowName
                //TODO: this should include multiple states but it makes segregating the query results tricky
                //TODO: relook at a later time
                new List<string> { this.StateName }, //List<string> workFlowStateNames
                this.StorageTable,
            };
            return (TaskBaseStatement)Activator.CreateInstance(this.TaskStatementType, parameters);
        }
    }
}
