using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Workflow.ThirdPartyInvoices;
using System;
using System.Linq;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_approving_third_party_invoice : WithFakes
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
        };

        private Because of = () =>
        {
            result = workflowProcess.ApproveThirdPartyInvoice(messages, thirdPartyInvoiceKey, metadata);
        };

        private It should_approve_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Param.IsAny<ApproveThirdPartyInvoiceCommand>(), metadata));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}