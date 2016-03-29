using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Move_from_Origination.OnComplete
{
    [Subject("Activity => Move_From_Origination => OnComplete")]
    internal class when_move_from_origination : WorkflowSpecCredit
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.EntryPath = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Move_from_Origination(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_entry_path_data_property = () =>
        {
            workflowData.EntryPath.ShouldEqual(2);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}