using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class DeleteActiveExternalActivityCommandHandler : IServiceCommandHandler<DeleteActiveExternalActivityCommand>
    {
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(DeleteActiveExternalActivityCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteByKey<ActiveExternalActivityDataModel, int>(command.ActiveExternalActivityID);
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}