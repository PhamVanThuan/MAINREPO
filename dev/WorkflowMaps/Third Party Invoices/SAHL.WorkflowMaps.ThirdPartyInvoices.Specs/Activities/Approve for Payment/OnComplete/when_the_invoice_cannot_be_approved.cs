using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.ThirdPartyInvoices;
using System;
using System.Linq;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Approve_for_Payment.OnComplete
{
    public class when_the_invoice_cannot_be_approved : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IThirdPartyInvoiceWorkflowProcess process;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
            {
                thirdPartyInvoiceKey = 985673;
                workflowData.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
                process = An<IThirdPartyInvoiceWorkflowProcess>();
                process.WhenToldTo(x => x.ApproveThirdPartyInvoice(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<int>(), paramsData.ServiceRequestMetadata))
                    .Return(false);
                domainServiceLoader.RegisterMockForType<IThirdPartyInvoiceWorkflowProcess>(process);
            };

        private Because of = () =>
            {
                var message = string.Empty;
                result = workflow.OnCompleteActivity_Approve_for_Payment(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_use_the_third_party_invoice_key_from_the_x2data = () =>
            {
                process.WasToldTo(x => x.ApproveThirdPartyInvoice(Param.IsAny<ISystemMessageCollection>(), workflowData.ThirdPartyInvoiceKey, paramsData.ServiceRequestMetadata));
            };

        private It should_send_the_x2_message_collection = () =>
            {
                process.WasToldTo(x => x.ApproveThirdPartyInvoice(messages, Param.IsAny<int>(), null));
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };
    }
}