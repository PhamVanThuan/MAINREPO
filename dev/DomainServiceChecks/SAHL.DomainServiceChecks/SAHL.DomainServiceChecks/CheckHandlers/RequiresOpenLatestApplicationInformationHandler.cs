using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresOpenLatestApplicationInformationHandler : IDomainCommandCheckHandler<IRequiresOpenLatestApplicationInformation>
    {
        private IApplicationDataManager applicationDataManager;

        public RequiresOpenLatestApplicationInformationHandler(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresOpenLatestApplicationInformation command)
        {
            ISystemMessageCollection systemMessage = SystemMessageCollection.Empty();
            bool isOpenApplication = applicationDataManager.IsLatestApplicationInformationOpen(command.ApplicationNumber);
            if (!isOpenApplication)
            {
                systemMessage.AddMessage(new SystemMessage("The latest application information for your application is not open.", SystemMessageSeverityEnum.Error));
            }

            return systemMessage;
        }
    }
}