using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.Interfaces.WorkflowTask.Queries;
using Task = SAHL.Services.Interfaces.WorkflowTask.Task;

namespace SAHL.Services.WorkflowTask.Server.QueryHandlers
{
    public class GetAssignedTasksForUserQueryHandler : IServiceQueryHandler<GetAssignedTasksForUserQuery>
    {
        private readonly ITaskQueryCoordinator taskQueryCoordinator;

        public GetAssignedTasksForUserQueryHandler(ITaskQueryCoordinator taskQueryCoordinator)
        {
            this.taskQueryCoordinator = taskQueryCoordinator;
        }

        public ISystemMessageCollection HandleQuery(GetAssignedTasksForUserQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            try
            {
                var results = taskQueryCoordinator.GetWorkflowTasks(query.Username, query.Capabilites);

                var result = new GetAssignedTasksForUserQueryResult
                {
                    BusinessProcesses = results.ToList(),
                };

                query.Result = new ServiceQueryResult<GetAssignedTasksForUserQueryResult>(new[] { result });
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("Failed to retrieve assigned tasks for user {0}, Exception: {1}", query.Username, ex);
                messages.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }
    }
}