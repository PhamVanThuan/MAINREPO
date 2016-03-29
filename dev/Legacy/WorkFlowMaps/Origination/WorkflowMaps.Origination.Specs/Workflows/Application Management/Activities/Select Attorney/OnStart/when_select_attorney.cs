using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Select_Attorney.OnStart
{
    [Subject("Activity => Select_Attorney => OnStart")] // AutoGenerated
    internal class when_select_attorney : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Select_Attorney(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}