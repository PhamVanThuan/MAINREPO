using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AcceptThirdPartyInvoice
{
    public class when_account_does_not_exist : ThirdPartyTestBase
    {
        private static SystemMessage errorMessage;

        private Establish context = () =>
        {
            errorMessage = new SystemMessage("Account does not exist", SystemMessageSeverityEnum.Error);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<CreateEmptyInvoiceCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<CreateThirdPartyInvoiceWorkflowCaseCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AcceptThirdPartyInvoiceCommand>()))
            .Callback<ISystemMessageCollection>(y =>
            {
                y.AddMessage(errorMessage);
            });
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
        };

        private It Should_excecute_registered_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(),
                Param<AcceptThirdPartyInvoiceCommand>.Matches(y => y.AccountNumber == accountNumber)));
        };

        private It Should_raise_a_third_party_invoice_rejected_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<ThirdPartyInvoiceSubmissionRejectedEvent>.
                Matches(y => y.AccountNumber == accountNumber && y.RejectionReasons.Contains(errorMessage.Message)), accountNumber,
                (int)GenericKeyType.Account, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_raise_a_third_party_invoice_accepted_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<ThirdPartyInvoiceSubmissionAcceptedEvent>(), Param.IsAny<int>(),
                Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_create_an_empty_invoice = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param<CreateEmptyInvoiceCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_save_invoice_attachment = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_create_an_X2_case = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<X2CreateInstanceWithCompleteRequest>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_raise_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}