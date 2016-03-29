using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ClientDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresClientCheckHandler : IDomainCommandCheckHandler<IRequiresClient>
    {
        private IClientDataManager clientDataManager;

        public RequiresClientCheckHandler(IClientDataManager clientDataManager)
        {
            this.clientDataManager = clientDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresClient command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            bool clientExists = clientDataManager.IsClientOnOurSystem(command.ClientKey);
            if (!clientExists)
            {
                systemMessages.AddMessage(new SystemMessage("The client provided, does not exist.", SystemMessageSeverityEnum.Error));
            }

            return systemMessages;
        }
    }
}