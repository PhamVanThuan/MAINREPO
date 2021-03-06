using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_45_day_Archive.OnExit
{
    [Subject("State => Common_45_day_Archive => OnExit")] // AutoGenerated
    internal class when_common_45_day_archive : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_45_day_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}