using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Close_Child_Instance.OnStart
{
    [Subject("Activity => Close_Child_Instance => OnStart")] // AutoGenerated
    internal class when_close_child_instance : WorkflowSpecLife
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Close_Child_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}