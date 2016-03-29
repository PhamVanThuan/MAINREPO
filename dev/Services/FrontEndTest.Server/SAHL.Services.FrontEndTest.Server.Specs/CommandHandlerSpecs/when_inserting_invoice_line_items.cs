using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FrontEndTest.CommandHandlers;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System.Collections.Generic;

namespace SAHL.Services.FrontEndTest.Server.Specs.CommandHandlerSpecs
{
    public class when_inserting_invoice_line_items : WithFakes
    {
        private static ITestDataManager testDataManager;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static InsertInvoiceLineItemsCommand command;
        private static InsertInvoiceLineItemsCommandHandler commandHandler;
        private static List<InvoiceLineItemDataModel> InvoiceLineItems;

        private Establish context = () =>
        {
            testDataManager = An<ITestDataManager>();
            metadata = An<IServiceRequestMetadata>();
            InvoiceLineItems = new List<InvoiceLineItemDataModel>();
            command = new InsertInvoiceLineItemsCommand(InvoiceLineItems);
            commandHandler = new InsertInvoiceLineItemsCommandHandler(testDataManager);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_insert_the_lineItems = () =>
        {
            testDataManager.WasToldTo(x => x.InsertInvoiceLineItems(InvoiceLineItems));
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}