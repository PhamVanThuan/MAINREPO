using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.SplitVal.OnStart
{
    [Subject("Activity => SplitVal => OnStart")] // AutoGenerated
    internal class when_splitval : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_SplitVal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}