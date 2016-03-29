using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Disbursed.OnExit
{
    [Subject("State => Disbursed => OnExit")]
    internal class when_disbursed : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Disbursed(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Disbursed");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}