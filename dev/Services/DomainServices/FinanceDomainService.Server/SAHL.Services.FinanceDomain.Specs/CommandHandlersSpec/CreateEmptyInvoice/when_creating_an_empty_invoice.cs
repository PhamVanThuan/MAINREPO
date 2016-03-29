using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using System;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CreateEmptyInvoice
{
    public class when_creating_an_empty_invoice : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceManager;
        private static CreateEmptyInvoiceCommandHandler handler;
        private static CreateEmptyInvoiceCommand command;
        private static ServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static ILinkedKeyManager linkedKeyManager;
        private static int accountKey;
        private static Guid correlationId;
        private static int thirdPartyInvoiceKey;
        private static string fromEmailAddress;
        private static DateTime receivedDate;

        private Establish context = () =>
        {
            fromEmailAddress = "test@sahomeloans.com";
            linkedKeyManager = An<ILinkedKeyManager>();
            thirdPartyInvoiceManager = An<IThirdPartyInvoiceDataManager>();
            handler = new CreateEmptyInvoiceCommandHandler(thirdPartyInvoiceManager, linkedKeyManager);
            metadata = new ServiceRequestMetadata();
            accountKey = 99;
            thirdPartyInvoiceKey = 78;
            correlationId = Guid.Parse("{D22EBA8E-A983-4AA3-855A-2C03C1B4D72E}");
            receivedDate = DateTime.Now;
            command = new CreateEmptyInvoiceCommand(accountKey, correlationId, fromEmailAddress, receivedDate);

            thirdPartyInvoiceManager.WhenToldTo(x => x.SaveEmptyThirdPartyInvoice(accountKey, correlationId, fromEmailAddress, receivedDate)).Return(thirdPartyInvoiceKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_save_an_empty_third_party_invoice = () =>
        {
            thirdPartyInvoiceManager.WasToldTo(x => x.SaveEmptyThirdPartyInvoice(accountKey, correlationId, fromEmailAddress, receivedDate));
        };

        private It should_add_third_party_key_to_linkedKeyManager = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(thirdPartyInvoiceKey, correlationId));
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}