using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.WorkflowTask;
using SAHL.Services.Interfaces.WorkflowTask.Commands;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.DataModels;
using System;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers
{
    public interface IWorkflowTaskDataManager
    {
        IEnumerable<WorkFlowStateItem> RetrieveWorkFlowStatesForWorkListItemsForUsername(string username, List<string> roles);
        void PerformTaskQueries(IEnumerable<ITaskQueryResultCollator> resultCollators);
        void AddTagForUser(CreateTagForUserCommand command);
        void AddTagToWorkFlowItem(AddTagToWorkflowInstanceCommand command);
        void DeleteAllInstancesOfTagOnWorkflowItems(Guid tagId);
        void DeleteTagForUser(Guid tagId);
        int GetWorkflowUserTagKey(Guid tagid, long workflowItemId);
        void DeleteWorkflowUserTagByKey(int keyId);
        void UpdateUserTag(UpdateUserTagCommand command);
        Guid FindTagIdForUsernameAndTextCombination(string tagText, string username);
        IEnumerable<UserTagsDataModel> GetAllTagsForUser(string userName);
        Dictionary<Guid, Tag> MapTags(IEnumerable<UserTagsDataModel> tags);
    }
}
