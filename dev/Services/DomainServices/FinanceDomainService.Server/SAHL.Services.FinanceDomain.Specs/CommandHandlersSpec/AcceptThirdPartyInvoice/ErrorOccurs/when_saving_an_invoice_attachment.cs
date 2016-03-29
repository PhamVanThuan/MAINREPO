using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using System;
using System.Linq;
using NSubstitute;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AcceptThirdPartyInvoice.ErrorOccurs
{
    internal class when_saving_an_invoice_attachment : ThirdPartyTestBase
    {
        private static ISystemMessageCollection errorMessages;

        private Establish context = () =>
        {
            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Error", SystemMessageSeverityEnum.Error));
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<CreateEmptyInvoiceCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument),
                Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
        };

        private It Should_create_an_empty_invoice = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<CreateEmptyInvoiceCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_save_invoice_attachment = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_create_an_X2_case = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<X2CreateInstanceWithCompleteRequest>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<IEvent>(), Param.IsAny<int>(),
                Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_return_errors = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_not_complete_the_unit_of_work = () =>
        {
            unitOfWork.DidNotReceive().Complete();
        };
    }
}