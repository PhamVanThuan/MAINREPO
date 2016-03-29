using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.States.Review_Decision.OnExit
{
    [Subject("State => Review_Decision => OnExit")]
    internal class when_review_decision : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Review_Decision(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property_to_review_decision = () =>
        {
            workflowData.PreviousState.ShouldEqual("Review Decision");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}