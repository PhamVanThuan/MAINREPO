using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.System_Assign_Processor.OnExit
{
    [Subject("State => System_Assign_Processor => OnExit")]
    internal class when_system_assign_processor : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_System_Assign_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_previous_state = () =>
        {
            workflowData.PreviousState.ShouldMatch("System Assign Processor");
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}