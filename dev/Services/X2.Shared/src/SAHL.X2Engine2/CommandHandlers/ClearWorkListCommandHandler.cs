﻿using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class ClearWorkListCommandHandler : IServiceCommandHandler<ClearWorkListCommand>
    {
        public ISystemMessageCollection HandleCommand(ClearWorkListCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteWhere<WorkListDataModel>("InstanceId=@InstanceId", new { InstanceId = command.InstanceID });
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}