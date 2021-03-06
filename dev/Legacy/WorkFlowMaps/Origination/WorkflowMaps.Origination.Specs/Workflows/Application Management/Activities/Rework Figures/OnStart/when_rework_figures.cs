using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Rework_Figures.OnStart
{
    [Subject("Activity => Rework_Figures => OnStart")] // AutoGenerated
    internal class when_rework_figures : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rework_Figures(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}