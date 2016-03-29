using Machine.Specifications;
using System;
using System.Linq;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.isLossControlInvoice.OnStart
{
    public class when_it_is_an_attorney_invoice : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ThirdPartyTypeKey = 1;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnStartActivity_isLossControlInvoice(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}