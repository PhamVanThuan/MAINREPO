using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.Older_than_45_Days.OnExit
{
    [Subject("State => Older_than_45_Days => OnExit")] // AutoGenerated
    internal class when_older_than_45_days : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Older_than_45_Days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}