using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Readvance_Post_Credit_Appro.OnComplete
{
    [Subject("Activity => Readvance_Post_Credit_Appro => OnComplete")]
    internal class when_readvance_post_credit_approval : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Readvance_Post_Credit_Appro(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_entry_path_to_2 = () =>
        {
            workflowData.EntryPath.ShouldEqual(2);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}