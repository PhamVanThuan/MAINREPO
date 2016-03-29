using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.EXTRefresh_Timer.OnStart
{
    [Subject("Activity => EXTRefresh_Timer => OnStart")] // AutoGenerated
    internal class when_extrefresh_timer : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_EXTRefresh_Timer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}