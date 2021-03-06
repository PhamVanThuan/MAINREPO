using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Reinstruct_Attorney.OnStart
{
    [Subject("Activity => Reinstruct_Attorney => OnStart")] // AutoGenerated
    internal class when_reinstruct_attorney : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Reinstruct_Attorney(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}