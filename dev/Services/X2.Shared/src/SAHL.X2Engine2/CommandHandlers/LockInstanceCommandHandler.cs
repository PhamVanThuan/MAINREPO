using System;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class LockInstanceCommandHandler : IServiceCommandHandler<LockInstanceCommand>
    {
        public ISystemMessageCollection HandleCommand(LockInstanceCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                command.Instance.ActivityADUserName = command.Username;
                command.Instance.ActivityID = command.Activity.ActivityID;
                command.Instance.ActivityDate = DateTime.Now;

                db.Update<InstanceDataModel>(command.Instance);
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}