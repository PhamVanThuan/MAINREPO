using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.DocumentManager.CommandHandlers;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.Interfaces.DocumentManager.Commands;
using System;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.CommandHandlers.RemoveAttorneyInvoice
{
    public class when_removing_an_attorney_invoice_data : WithFakes
    {
        private static RemoveAttorneyInvoiceCommandHandler handler;
        private static RemoveAttorneyInvoiceCommand command;

        private static IDocumentDataManager dataManager;
        private static ServiceRequestMetadata metadata;

        private static ISystemMessageCollection messages;
        private static int AttorneyInvoiceKey;

        private Establish context = () =>
        {
            AttorneyInvoiceKey = 11001;

            dataManager = An<IDocumentDataManager>();
            metadata = new ServiceRequestMetadata();
            command = new RemoveAttorneyInvoiceCommand(AttorneyInvoiceKey);
            handler = new RemoveAttorneyInvoiceCommandHandler(dataManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_delete_the_attorney_invoice = () =>
        {
            dataManager.WasToldTo(x => x.RemoveAttorneyInvoice(command.AttorneyInvoiceKey));
        };

        private It should_not_return_any_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}