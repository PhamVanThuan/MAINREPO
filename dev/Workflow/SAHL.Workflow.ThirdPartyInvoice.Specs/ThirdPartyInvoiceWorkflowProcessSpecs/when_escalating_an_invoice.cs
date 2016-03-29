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
    public class when_escalating_an_invoice : WithFakes
    {
        private static IThirdPartyInvoiceWorkflowProcess workflowProcess;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int thirdPartyInvoiceKey;
        private static bool result;
        private static int uosKeyEscalatedTo;
        private Establish context = () =>
        {
            uosKeyEscalatedTo = 35864;
            financeDomainServiceClient = An<IFinanceDomainServiceClient>();
            workflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            metadata = An<IServiceRequestMetadata>();

            thirdPartyInvoiceKey = 232;
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            result = workflowProcess.EscalateThirdPartyInvoiceForApproval(messages, thirdPartyInvoiceKey, uosKeyEscalatedTo, metadata);
        };

        private It should_query_the_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Arg.Is<EscalateThirdPartyInvoiceForApprovalCommand>
                (y=>y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey && y.UOSKeyForEscalatedUser == uosKeyEscalatedTo), metadata));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}