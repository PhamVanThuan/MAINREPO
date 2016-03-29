using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Disputes.OnExit
{
    [Subject("State => Disputes => OnExit")]
    internal class when_disputes : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            workflowData.PreviousState = "Test";
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Disputes(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_propetry = () =>
        {
            workflowData.PreviousState.ShouldMatch("Disputes");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}