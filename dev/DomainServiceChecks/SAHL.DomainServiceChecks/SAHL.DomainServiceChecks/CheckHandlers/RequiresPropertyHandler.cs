using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.PropertyDataManager;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresPropertyCheckHandler : IDomainCommandCheckHandler<IRequiresProperty>
    {
        private IPropertyDataManager PropertyDataManager { get; set; }

        public RequiresPropertyCheckHandler(IPropertyDataManager propertyDataManager)
        {
            this.PropertyDataManager = propertyDataManager;
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresProperty command)
        {
            ISystemMessageCollection systemMessage = SystemMessageCollection.Empty();
            bool propertyExists = PropertyDataManager.IsPropertyOnOurSystem(command.PropertyKey);
            if (!propertyExists)
            {
                systemMessage.AddMessage(new SystemMessage("No property could be found against your property number.", SystemMessageSeverityEnum.Error));
            }
            return systemMessage;
        }
    }
}