using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_removing_an_empty_third_party_invoice : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static RemoveEmptyThirdPartyInvoiceCommand command;
        private static RemoveEmptyThirdPartyInvoiceCommandHandler commandHandler;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static int thirdPartyInvoiceKey;
        private static EmptyThirdPartyInvoicesDataModel thirdPartyInvoice;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            thirdPartyInvoiceKey = 123;
            command = new RemoveEmptyThirdPartyInvoiceCommand(thirdPartyInvoiceKey);
            commandHandler = new RemoveEmptyThirdPartyInvoiceCommandHandler(testDataManager);
            thirdPartyInvoice = new EmptyThirdPartyInvoicesDataModel(123, thirdPartyInvoiceKey);
            thirdPartyInvoice.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_remove_the_correct_empty_invoice = () =>
        {
            testDataManager.WasToldTo(x => x.RemoveEmptyThirdPartyInvoice(thirdPartyInvoice.ThirdPartyInvoiceKey));
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}