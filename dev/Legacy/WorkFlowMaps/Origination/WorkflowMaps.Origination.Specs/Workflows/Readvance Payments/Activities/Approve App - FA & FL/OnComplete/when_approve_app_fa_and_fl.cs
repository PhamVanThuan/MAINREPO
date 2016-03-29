using Machine.Specifications;
using WorkflowMaps.ReadvancePayments.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Approve_App___FA___FL.OnComplete
{
    [Subject("Activity => Approve_App___FA___FL => OnComplete")]
    internal class when_approve_app_fa_and_fl : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
            {
                workflowData.EntryPath = 0;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Approve_App___FA___FL(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_set_entry_path_data_property = () =>
            {
                workflowData.EntryPath.ShouldEqual(3);
            };

        private It result_should_be_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}