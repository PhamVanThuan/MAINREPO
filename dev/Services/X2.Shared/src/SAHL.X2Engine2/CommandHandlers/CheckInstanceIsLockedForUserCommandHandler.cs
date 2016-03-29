using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CheckInstanceIsLockedForUserCommandHandler : IServiceCommandHandler<CheckInstanceIsLockedForUserCommand>
    {
        public ISystemMessageCollection HandleCommand(CheckInstanceIsLockedForUserCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                InstanceDataModel instance = db.SelectOne<InstanceDataModel>(UIStatements.instancedatamodel_selectbykey, new { PrimaryKey = command.InstanceId });
                db.Complete();
                if (instance.ActivityID == command.ActivityId && instance.ActivityADUserName == command.UserName)
                {
                    command.Result = true;
                }
                else
                {
                    command.Result = false;
                }
            }
            return new SystemMessageCollection();
        }
    }
}