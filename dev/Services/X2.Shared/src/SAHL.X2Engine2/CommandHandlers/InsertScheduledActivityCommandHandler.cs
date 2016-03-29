using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class InsertScheduledActivityCommandHandler : IServiceCommandHandler<InsertScheduledActivityCommand>
    {
        public ISystemMessageCollection HandleCommand(InsertScheduledActivityCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                ScheduledActivityDataModel model = new ScheduledActivityDataModel(command.InstanceId, command.TimeToExecute, command.ActivityDataModel.ID, command.ActivityDataModel.Priority, command.WorkflowProviderName);
                db.Insert<ScheduledActivityDataModel>(model);
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}