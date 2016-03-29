﻿using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class DeleteAllScheduleActivitiesCommandHandler : IServiceCommandHandler<DeleteAllScheduleActivitiesCommand>
    {
        private IX2ServiceCommandRouter commandHandler;

        public DeleteAllScheduleActivitiesCommandHandler(IX2ServiceCommandRouter commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public ISystemMessageCollection HandleCommand(DeleteAllScheduleActivitiesCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteWhere<ScheduledActivityDataModel>("InstanceID=@InstanceID", new { InstanceID = command.InstanceId });
                db.Complete();
            }
            return new SystemMessageCollection();
        }
    }
}