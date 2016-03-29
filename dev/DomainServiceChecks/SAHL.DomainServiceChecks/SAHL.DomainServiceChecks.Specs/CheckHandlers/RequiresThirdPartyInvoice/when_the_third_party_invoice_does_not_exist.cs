using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresThirdPartyInvoice
{
    public class when_the_third_party_invoice_does_not_exist : WithFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static RequiresThirdPartyInvoiceHandler handler;
        private static CommandToCheck commandCheck;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            dataManager = An<IThirdPartyInvoiceDataManager>();
            commandCheck = new CommandToCheck(Guid.NewGuid(), 1234);
            handler = new RequiresThirdPartyInvoiceHandler(dataManager);
            dataManager.WhenToldTo(x => x.DoesThirdPartyInvoiceExist(Arg.Any<int>())).Return(false);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(commandCheck);
        };

        private It should_return_an_error_message = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldContain(
                string.Format("No Third Party Invoice with Key {0} exists.", commandCheck.ThirdPartyInvoiceKey));
        };
    }
}