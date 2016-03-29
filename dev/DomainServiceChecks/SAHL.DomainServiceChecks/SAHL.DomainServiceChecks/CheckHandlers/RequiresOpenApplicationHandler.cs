using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresOpenApplicationCheckHandler : IDomainCommandCheckHandler<IRequiresOpenApplication>
    {
        private IApplicationDataManager applicationDataManager;

        public RequiresOpenApplicationCheckHandler(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresOpenApplication command)
        {
            ISystemMessageCollection systemMessage = SystemMessageCollection.Empty();
            bool isOpenApplication = applicationDataManager.IsApplicationOpen(command.ApplicationNumber);
            if (!isOpenApplication)
            {
                systemMessage.AddMessage(new SystemMessage("No open application could be found against your application number.", SystemMessageSeverityEnum.Error));
            }

            return systemMessage;
        }
    }
}