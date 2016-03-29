using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Readvance_Payment.OnComplete
{
    [Subject("Activity => Readvance_Payment => OnComplete")]
    internal class when_readvance_payment : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 2;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Readvance_Payment(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_entry_path_to_1 = () =>
        {
            workflowData.EntryPath.ShouldEqual(1);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}