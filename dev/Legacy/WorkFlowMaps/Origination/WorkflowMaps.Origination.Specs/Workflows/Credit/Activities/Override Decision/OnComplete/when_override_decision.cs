using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Override_Decision.OnComplete
{
    [Subject("Activity => Override_Decision => OnComplete")]
    internal class when_override_decision : WorkflowSpecCredit
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.StopRecursing = true;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Override_Decision(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_stop_recursing_data_property = () =>
        {
            workflowData.StopRecursing = false;
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}