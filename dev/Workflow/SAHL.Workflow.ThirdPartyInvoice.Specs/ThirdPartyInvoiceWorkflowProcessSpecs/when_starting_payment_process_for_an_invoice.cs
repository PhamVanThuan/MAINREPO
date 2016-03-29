using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Workflow.ThirdPartyInvoices;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_starting_payment_process_for_an_invoice : WithFakes
    {
        private static IThirdPartyInvoiceWorkflowProcess workflowProcess;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int thirdPartyInvoiceKey;
        private static bool result;

        private Establish context = () =>
        {
            financeDomainServiceClient = An<IFinanceDomainServiceClient>();
            workflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            metadata = An<IServiceRequestMetadata>();

            thirdPartyInvoiceKey = 232;
            messages = SystemMessageCollection.Empty();

            var emptyMessageCollection = SystemMessageCollection.Empty();
            financeDomainServiceClient.WhenToldTo(x => x.PerformCommand(Param<ProcessThirdPartyInvoicePaymentCommand>.Matches(y =>
                y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey
                ), metadata)
              ).Return(emptyMessageCollection);
        };

        private Because of = () =>
        {
            result = workflowProcess.ProcessInvoicePayment(messages, thirdPartyInvoiceKey, metadata);
        };

        private It should_approve_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Param<ProcessThirdPartyInvoicePaymentCommand>.Matches(y =>
                y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey
                ), metadata));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}