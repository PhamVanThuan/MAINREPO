using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresActiveClientRoleHandler : IDomainCommandCheckHandler<IRequiresActiveClientRole>
    {
        private IApplicationDataManager applicationDataManager;

        public RequiresActiveClientRoleHandler(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresActiveClientRole command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            bool isActiveClientRole = applicationDataManager.IsActiveClientRole(command.ApplicationRoleKey);
            if (!isActiveClientRole)
            {
                systemMessages.AddMessage(new SystemMessage("There is no active client role for the provided offer role.", SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}