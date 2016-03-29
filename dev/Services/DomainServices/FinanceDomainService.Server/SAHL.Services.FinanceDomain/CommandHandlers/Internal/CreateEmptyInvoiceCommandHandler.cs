using Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;

namespace SAHL.Services.FinanceDomain.CommandHandlers.Internal
{
    public class CreateEmptyInvoiceCommandHandler : IServiceCommandHandler<CreateEmptyInvoiceCommand>
    {
        private IThirdPartyInvoiceDataManager thirdPartyInvoiceManager;
        private ILinkedKeyManager LinkedKeyManager;

        public CreateEmptyInvoiceCommandHandler(IThirdPartyInvoiceDataManager thirdPartyInvoiceManager, ILinkedKeyManager linkedKeyManager)
        {
            this.thirdPartyInvoiceManager = thirdPartyInvoiceManager;
            this.LinkedKeyManager = linkedKeyManager;
        }

        public ISystemMessageCollection HandleCommand(CreateEmptyInvoiceCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            int thirdPartyInvoiceKey = thirdPartyInvoiceManager.SaveEmptyThirdPartyInvoice(command.AccountKey, command.EmptyInvoiceGuid, command.ReceivedFromEmailAddress,command.ReceivedDate);
            LinkedKeyManager.LinkKeyToGuid(thirdPartyInvoiceKey, command.EmptyInvoiceGuid);

            return messages;
        }
    }
}