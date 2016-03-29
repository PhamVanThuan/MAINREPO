using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ReverseInvoicePaymentProcess
{
    public class when_reversing_invoice_payment_process : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static ReturnThirdPartyInvoiceToPaymentQueueCommandHandler commandHandler;
        private static ReturnThirdPartyInvoiceToPaymentQueueCommand command;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            serviceRequestMetaData = An<IServiceRequestMetadata>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceKey = 78342;
            command = new ReturnThirdPartyInvoiceToPaymentQueueCommand(thirdPartyInvoiceKey);
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            commandHandler = new ReturnThirdPartyInvoiceToPaymentQueueCommandHandler(thirdPartyInvoiceDataManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_change_invoice_status_to_approved = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Approved));
        };

        private It should_raise_the_event = () =>
        {
            eventRaiser.Received().RaiseEvent(Arg.Any<DateTime>(), Arg.Any<ThirdPartyInvoiceReturnedToPaymentQueueEvent>(), command.ThirdPartyInvoiceKey,
                (int)GenericKeyType.ThirdPartyInvoice, serviceRequestMetaData);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}