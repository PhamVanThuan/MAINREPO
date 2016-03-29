using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AcceptThirdPartyInvoice
{
    public class when_account_exist : ThirdPartyTestBase
    {
        public Establish context = () =>
        {
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<CreateEmptyInvoiceCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<CreateThirdPartyInvoiceWorkflowCaseCommand>.Matches(y => y.AccountKey == accountNumber),
                Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<AcceptThirdPartyInvoiceCommand>()))
            .Callback<ISystemMessageCollection>(y => { SystemMessageCollection.Empty(); });
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_use_a_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It Should_execute_registered_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param<AcceptThirdPartyInvoiceCommand>.Matches(y => y.AccountNumber == accountNumber)));
        };

        private It Should_create_an_empty_invoice = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<CreateEmptyInvoiceCommand>.Matches(y => y.AccountKey == accountNumber
                && y.ReceivedFromEmailAddress == command.InvoiceDocument.FromEmailAddress),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_save_invoice_attachment = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_use_the_linked_key_in_subsequent_commands = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<AddInvoiceAttachmentCommand>.Matches(y => y.InvoiceDocument == invoiceDocument
                && y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_create_an_X2_case = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<CreateThirdPartyInvoiceWorkflowCaseCommand>.Matches(y =>
                    y.ReceivedFrom == command.InvoiceDocument.FromEmailAddress
                    && y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey
                    && y.ThirdPartyTypeKey == (int)ThirdPartyType.Attorney),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_raise_a_third_party_invoice_accepted_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<ThirdPartyInvoiceSubmissionAcceptedEvent>.
                Matches(y => y.AccountNumber == accountNumber && y.InvoiceDocument == invoiceDocument),
                 Param.IsAny<int>(), (int)GenericKeyType.ThirdPartyInvoice, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_raise_a_third_party_invoice_rejected_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<ThirdPartyInvoiceSubmissionRejectedEvent>(), Param.IsAny<int>(),
                Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It Should_not_return_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It Should_complete_the_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}