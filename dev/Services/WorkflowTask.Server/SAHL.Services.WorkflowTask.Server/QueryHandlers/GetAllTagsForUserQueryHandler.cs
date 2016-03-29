using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.Interfaces.WorkflowTask.Queries;
using SAHL.Services.WorkflowTask.Server.Managers;
using System;

namespace SAHL.Services.WorkflowTask.Server.QueryHandlers
{
    public class GetAllTagsForUserQueryHandler : IServiceQueryHandler<GetAllTagsForUserQuery>
    {
        private readonly IWorkflowTaskDataManager dataManager;

        public GetAllTagsForUserQueryHandler(WorkflowTaskDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ISystemMessageCollection HandleQuery(GetAllTagsForUserQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            try
            {
                var tags = dataManager.GetAllTagsForUser(query.UserName);
                var returningTags = dataManager.MapTags(tags);
                query.Result = new ServiceQueryResult<GetAllTagsForUserQueryResult>(new[] { new GetAllTagsForUserQueryResult { Tags = returningTags } });
            }
            catch (Exception ex)
            {
                var error = string.Format("Failed to fetch all tags for user {0}{1}{2}", query.UserName, Environment.NewLine, ex);
                messages.AddMessage(new SystemMessage(error, SystemMessageSeverityEnum.Error));
            }
            return messages;
        }
    }
}