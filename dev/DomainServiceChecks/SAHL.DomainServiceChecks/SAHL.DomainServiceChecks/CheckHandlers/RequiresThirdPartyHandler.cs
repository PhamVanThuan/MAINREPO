using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using System;
using System.Linq;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresThirdPartyHandler : IDomainCommandCheckHandler<IRequiresThirdParty>
    {
        private IThirdPartyInvoiceDataManager dataManager;

        public RequiresThirdPartyHandler(IThirdPartyInvoiceDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCheckCommand(IRequiresThirdParty command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            bool exists = this.dataManager.DoesThirdPartyExist(command.ThirdPartyId);
            if (!exists)
            {
                systemMessages.AddMessage(new SystemMessage(string.Format(@"No Third Party with Id {0} exists.", command.ThirdPartyId), SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}