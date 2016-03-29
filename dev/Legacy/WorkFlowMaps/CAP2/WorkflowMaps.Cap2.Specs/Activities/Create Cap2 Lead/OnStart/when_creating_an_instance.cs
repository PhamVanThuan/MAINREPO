using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Create_Cap2_Lead.OnStart
{
    [Subject("Activity => Create_Cap2_Lead => OnStart")]
    internal class when_creating_an_instance : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_CAP2_lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}