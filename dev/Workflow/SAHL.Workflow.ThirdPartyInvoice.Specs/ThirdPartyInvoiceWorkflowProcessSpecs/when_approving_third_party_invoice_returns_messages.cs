using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Workflow.ThirdPartyInvoices;
using System;
using System.Linq;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_approving_third_party_invoice_returns_messages : WithFakes
    {
        private static IThirdPartyInvoiceWorkflowProcess workflowProcess;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int thirdPartyInvoiceKey;
        private static bool result;
        private static SystemMessage errorMessage;

        private Establish context = () =>
        {
            financeDomainServiceClient = An<IFinanceDomainServiceClient>();
            var errorMessages = SystemMessageCollection.Empty();
            errorMessage = new SystemMessage("Error", SystemMessageSeverityEnum.Error);
            errorMessages.AddMessage(errorMessage);
            financeDomainServiceClient.WhenToldTo(x => x.PerformCommand(Arg.Any<ApproveThirdPartyInvoiceCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);
            workflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            metadata = An<IServiceRequestMetadata>();

            thirdPartyInvoiceKey = 232;
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            result = workflowProcess.ApproveThirdPartyInvoice(messages, thirdPartyInvoiceKey, metadata);
        };

        private It should_approve_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Param.IsAny<ApproveThirdPartyInvoiceCommand>(), metadata));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_contain_the_error_message_in_the_message_collection = () =>
        {
            messages.ErrorMessages().First().ShouldEqual(errorMessage);
        };
    }
}