using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.AccountDataManager;
using SAHL.DomainServiceChecks.Managers.X2InstanceDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresX2InstanceHandler : IDomainCommandCheckHandler<IRequiresX2Instance>
    {
        private IX2DataManager dataManager;

        public RequiresX2InstanceHandler(IX2DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ISystemMessageCollection HandleCheckCommand(IRequiresX2Instance command)
        {
           var messages = SystemMessageCollection.Empty();
           if(!dataManager.DoesInstanceIdExist(command.InstanceId))
           {
              messages.AddMessage(new SystemMessage(string.Format("No X2 Instance with Id {0} exists.", command.InstanceId), SystemMessageSeverityEnum.Error));
           }
           return messages;
        }

    }
}