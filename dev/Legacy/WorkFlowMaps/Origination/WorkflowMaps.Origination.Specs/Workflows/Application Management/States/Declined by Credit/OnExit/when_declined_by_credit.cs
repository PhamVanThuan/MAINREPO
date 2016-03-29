using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Declined_by_Credit.OnExit
{
    [Subject("State => Declined_by_Credit => OnExit")]
    internal class when_declined_by_credit : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state_property = () =>
        {
            workflowData.PreviousState.ShouldMatch("Declined by Credit");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}