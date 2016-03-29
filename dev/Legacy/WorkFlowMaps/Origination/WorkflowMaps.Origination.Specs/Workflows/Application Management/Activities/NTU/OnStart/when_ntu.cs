using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.NTU.OnStart
{
    [Subject("Activity => NTU => OnStart")] // AutoGenerated
    internal class when_ntu : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}