using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_updating_a_third_party_invoice : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static ITestDataManager testDataManager;
        private static UpdateThirdPartyInvoiceCommand command;
        private static ThirdPartyInvoiceDataModel model;
        private static int accountKey;
        private static UpdateThirdPartyInvoiceCommandHandler commandHandler;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            accountKey = 1235888;
            model = new ThirdPartyInvoiceDataModel("sahlReference", 5, accountKey, Guid.NewGuid(), "invoiceNumber", DateTime.Now, "receivedFromEmailAddress", 10, 1, 8, true, DateTime.Now, "paymentReference");
            command = new UpdateThirdPartyInvoiceCommand(model);
            testDataManager = An<ITestDataManager>();
            commandHandler = new UpdateThirdPartyInvoiceCommandHandler(testDataManager);
            metadata = An<IServiceRequestMetadata>();
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_use_the_update_statement_through_the_testDataManager_to_update_the_variableLoanInfo = () =>
        {
            testDataManager.WasToldTo(x => x.UpdateThirdPartyInvoice(model));
        };

        private It should_return_messages = () =>
        {
            messages.ShouldNotBeNull();
        };

        private It should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}