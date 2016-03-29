using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class DeleteScheduledActivityCommandHandler : IServiceCommandHandler<DeleteScheduledActivityCommand>
    {
        public ISystemMessageCollection HandleCommand(DeleteScheduledActivityCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteWhere<ScheduledActivityDataModel>("InstanceID=@InstanceId and ActivityId=@ActivityId", new { InstanceId = command.InstanceId, ActivityId = command.ActivityId });
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}