using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Wait_for_Followup.OnStart
{
    [Subject("Activity => Wait_for_Followup => OnStart")] // AutoGenerated
    internal class when_wait_for_followup : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Wait_for_Followup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}