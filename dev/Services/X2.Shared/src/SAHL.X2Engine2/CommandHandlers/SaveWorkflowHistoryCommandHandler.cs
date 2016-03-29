using System;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class SaveWorkflowHistoryCommandHandler : IServiceCommandHandler<SaveWorkflowHistoryCommand>
    {
        public ISystemMessageCollection HandleCommand(SaveWorkflowHistoryCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                // need to load instance here
                WorkFlowHistoryDataModel workFlowHistoryDataModel = new WorkFlowHistoryDataModel(command.Instance.ID, command.ToStateID, command.ActivityID, command.UserWhoPerformedTheActivity,
                    command.Instance.CreationDate, DateTime.Now, command.Instance.DeadlineDate, DateTime.Now, command.UserWhoPerformedTheActivity, command.Instance.Priority);
                db.Insert<WorkFlowHistoryDataModel>(workFlowHistoryDataModel);
                command.WorkflowHistoryId = workFlowHistoryDataModel.ID;
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}