using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using System;

namespace SAHL.DomainServiceChecks.CheckHandlers
{
    public class RequiresThirdPartyInvoiceHandler : IDomainCommandCheckHandler<IRequiresThirdPartyInvoice>
    {
        private IThirdPartyInvoiceDataManager dataManager;

        public RequiresThirdPartyInvoiceHandler(IThirdPartyInvoiceDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCheckCommand(IRequiresThirdPartyInvoice command)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();
            var exists = this.dataManager.DoesThirdPartyInvoiceExist(command.ThirdPartyInvoiceKey);
            if (!exists)
            {
                systemMessages.AddMessage(new SystemMessage(string.Format(@"No Third Party Invoice with Key {0} exists.", command.ThirdPartyInvoiceKey),
                    SystemMessageSeverityEnum.Error));
            }
            return systemMessages;
        }
    }
}