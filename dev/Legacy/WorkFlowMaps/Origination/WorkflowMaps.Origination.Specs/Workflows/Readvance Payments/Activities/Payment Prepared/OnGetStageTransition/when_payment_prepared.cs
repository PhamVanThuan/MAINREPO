using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Payment_Prepared.OnGetStageTransition
{
    [Subject("Activity => Payment_Prepared => OnGetStageTransition")]
    internal class when_payment_prepared : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Payment_Prepared(instanceData, workflowData, paramsData, messages);
        };

        private It should_payment_prepared_string = () =>
        {
            result.ShouldBeTheSameAs("Payment Prepared");
        };
    }
}