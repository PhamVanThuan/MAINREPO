using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ThirdPartyInvoiceDataManager;
using System;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresThirdPartyInvoice
{
    public class when_the_third_party_invoice_exists : WithFakes
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
            dataManager.WhenToldTo(x => x.DoesThirdPartyInvoiceExist(Arg.Any<int>())).Return(true);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(commandCheck);
        };

        private It should_not_return_any_messages = () =>
        {
            systemMessages.AllMessages.ShouldBeEmpty();
        };

        private It should_pass_the_command_thirdPartyInvoiceKey_to_the_data_manager = () =>
        {
            dataManager.Received().DoesThirdPartyInvoiceExist(Arg.Is(commandCheck.ThirdPartyInvoiceKey));
        };
    }
}