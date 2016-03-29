using Machine.Specifications;
using System;
using System.Linq;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.isLossControlInvoice.OnComplete
{
    public class when_is_loss_control_invoice : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_isLossControlInvoice(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}