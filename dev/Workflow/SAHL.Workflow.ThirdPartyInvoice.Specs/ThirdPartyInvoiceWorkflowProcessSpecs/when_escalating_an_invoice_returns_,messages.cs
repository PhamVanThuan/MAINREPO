using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Workflow.ThirdPartyInvoices;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_escalating_an_invoice_returns_messages : WithFakes
    {
        private static IThirdPartyInvoiceWorkflowProcess workflowProcess;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int thirdPartyInvoiceKey;
        private static bool result;
        private static SystemMessage errorMessage;
        private static int uosKeyEscalatedTo;

        private Establish context = () =>
        {
            uosKeyEscalatedTo = 45291;
            financeDomainServiceClient = An<IFinanceDomainServiceClient>();
            var errorMessages = SystemMessageCollection.Empty();
            errorMessage = new SystemMessage("Error", SystemMessageSeverityEnum.Error);
            errorMessages.AddMessage(errorMessage);
            financeDomainServiceClient.WhenToldTo(x => x.PerformCommand(Arg.Any<EscalateThirdPartyInvoiceForApprovalCommand>(),
                Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);
            workflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            metadata = An<IServiceRequestMetadata>();

            thirdPartyInvoiceKey = 232;
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            result = workflowProcess.EscalateThirdPartyInvoiceForApproval(messages, thirdPartyInvoiceKey, uosKeyEscalatedTo, metadata);
        };

        private It should_approve_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Param.IsAny<EscalateThirdPartyInvoiceForApprovalCommand>(), metadata));
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