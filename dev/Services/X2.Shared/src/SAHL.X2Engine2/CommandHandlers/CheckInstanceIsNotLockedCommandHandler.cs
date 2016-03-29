using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Commands;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class CheckInstanceIsNotLockedCommandHandler : IServiceCommandHandler<CheckInstanceIsNotLockedCommand>
    {
        public ISystemMessageCollection HandleCommand(CheckInstanceIsNotLockedCommand command, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InWorkflowContext())
            {
                InstanceDataModel instance = db.SelectOne<InstanceDataModel>(UIStatements.instancedatamodel_selectbykey, new { PrimaryKey = command.InstanceId });
                db.Complete();
                if ((instance.ActivityADUserName == command.UserName) ||//same user locked it so thats fine
                    (string.IsNullOrEmpty(instance.ActivityADUserName)))// not locked also fine
                {
                    command.Result = true;
                    return new SystemMessageCollection();
                }
                else
                {
                    command.Result = false;
                    var messages = new SystemMessageCollection();
                    messages.AddMessage(new SystemMessage(string.Format("{0} is currently working on this case.", instance.ActivityADUserName), SystemMessageSeverityEnum.Error));
                    return messages;
                }
            }
        }
    }
}