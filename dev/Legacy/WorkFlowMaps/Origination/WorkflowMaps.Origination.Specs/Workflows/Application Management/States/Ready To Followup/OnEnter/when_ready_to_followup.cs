using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Ready_To_Followup.OnEnter
{
    [Subject("State => Ready_To_Followup => OnEnter")] // AutoGenerated
    internal class when_ready_to_followup : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Ready_To_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}