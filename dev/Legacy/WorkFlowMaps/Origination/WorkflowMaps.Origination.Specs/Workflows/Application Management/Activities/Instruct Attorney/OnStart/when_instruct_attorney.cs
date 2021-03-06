using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Instruct_Attorney.OnStart
{
    [Subject("Activity => Instruct_Attorney => OnStart")] // AutoGenerated
    internal class when_instruct_attorney : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Instruct_Attorney(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}