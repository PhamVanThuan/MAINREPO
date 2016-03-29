using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Workflow.ThirdPartyInvoices;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_archiving_an_invoice_returns_error_messages : WithFakes
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

            var errorMessageCollection = SystemMessageCollection.Empty();
            errorMessageCollection.AddMessage(new SystemMessage("An error occured", SystemMessageSeverityEnum.Error));

            financeDomainServiceClient.WhenToldTo(x => x.PerformCommand(Param<MarkThirdPartyInvoiceAsPaidCommand>.Matches(y =>
                y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey
                ), metadata)
              ).Return(errorMessageCollection);
        };

        private Because of = () =>
        {
            result = workflowProcess.ArchiveThirdPartyInvoice(messages, thirdPartyInvoiceKey, metadata);
        };

        private It should_approve_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Param<MarkThirdPartyInvoiceAsPaidCommand>.Matches(y =>
                y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey
                ), metadata));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}