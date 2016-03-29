using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.ThirdPartyInvoices;
using System;
using System.Linq;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Approve_for_Payment.OnComplete
{
    public class when_the_invoice_can_be_approved : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IThirdPartyInvoiceWorkflowProcess process;

        private Establish context = () =>
        {
            process = An<IThirdPartyInvoiceWorkflowProcess>();
            process.WhenToldTo(x => x.ApproveThirdPartyInvoice(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<int>(), paramsData.ServiceRequestMetadata))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IThirdPartyInvoiceWorkflowProcess>(process);
        };

        private Because of = () =>
        {
            var message = string.Empty;
            result = workflow.OnCompleteActivity_Approve_for_Payment(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}