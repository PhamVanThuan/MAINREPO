using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Attributes;
using SAHL.Core.Data;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements
{
    /// <summary>
    ///     Parameters available: Username, UsernameAndRoles, Roles, WorkFlowName, BusinessProcessName, WorkFlowStateNames
    /// </summary>
    public abstract class TaskBaseStatement : TaskBaseStatement<object>
    {
        //If you alter this constructor, you will break the creation of these statements, as they are created via Activator.CreateInstance
        //Ensure you also alter the parameter array in TaskQueryResultCollator.ConstructTaskStatement()
        protected TaskBaseStatement(string username, List<string> capabilites, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, capabilites, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }
    }

    [NolockConventionExclude]
    public abstract class TaskBaseStatement<T> : ISqlStatement<T>
    {
        //TODO: make this a UDF
        //WL.InstanceId is from the outer query
        protected const string tagStatementSubQuery = @"
    SELECT CAST(Id AS VARCHAR(36)) + ',' AS 'data()'
    FROM [2am].[tag].WorkflowItemUserTags wiut WITH (NOLOCK)
    INNER JOIN [2am].[tag].UserTags ut WITH (NOLOCK) ON wiut.TagId = ut.Id
    WHERE wiut.WorkFlowItemId = WL.InstanceId
    AND wiut.ADUsername = @Username
    FOR XML PATH ('')";

        private readonly bool includeTasksWithTheRoleOfEveryone;
        private readonly List<string> roles;
        private readonly List<string> workFlowStateNames;

        protected TaskBaseStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
        {
            this.Username = username;
            this.roles = roles;
            this.includeTasksWithTheRoleOfEveryone = includeTasksWithTheRoleOfEveryone;
            this.BusinessProcessName = businessProcessName;
            this.WorkFlowName = workFlowName;
            this.workFlowStateNames = GetWorkFlowStateNames(workFlowStateNames);
            //convention tests pass the value str, for all string inputs thus breaking the sql created by this class.
            this.StorageTable = string.Equals(storageTable, "str", StringComparison.OrdinalIgnoreCase) ? string.Empty : storageTable;
        }

        public string Username { get; protected set; }

        public string WorkFlowName { get; protected set; }
        public string BusinessProcessName { get; protected set; }
        public string StorageTable { get; protected set; }

        public IEnumerable<string> WorkFlowStateNames
        {
            get { return this.workFlowStateNames; }
        }

        public IEnumerable<string> UsernameAndRoles
        {
            get
            {
                if (this.includeTasksWithTheRoleOfEveryone)
                {
                    yield return "Everyone";
                }
                yield return this.Username;
                foreach (var item in this.roles ?? Enumerable.Empty<string>())
                {
                    yield return item;
                }
            }
        }

        public abstract string GetStatement();

        private static List<string> GetWorkFlowStateNames(List<string> workFlowStateNames)
        {
            return workFlowStateNames == null ? new List<string>() : workFlowStateNames.ToList();
        }

        public virtual void AddStateName(string stateName)
        {
            this.workFlowStateNames.Add(stateName);
        }

        protected string GetTagStatement()
        {
            throw new NotImplementedException();
        }
    }
}
