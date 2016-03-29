using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.LOA.OnExit
{
    [Subject("State => LOA => OnExit")]
    internal class when_loa : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_LOA(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("LOA");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}