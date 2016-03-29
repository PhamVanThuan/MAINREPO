using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Signed_LOA_Review.OnExit
{
    [Subject("State => Signed_LOA_Review => OnExit")]
    internal class when_signed_loa_review : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Signed_LOA_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Signed LOA Review");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}