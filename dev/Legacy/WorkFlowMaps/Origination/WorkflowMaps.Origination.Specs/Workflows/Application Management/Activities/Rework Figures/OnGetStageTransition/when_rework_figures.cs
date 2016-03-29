using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rework_Figures.OnGetStageTransition
{
    [Subject("Activity => Rework_Figures => OnGetStageTransition")]
    internal class when_rework_figures : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Rework_Figures(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_rework_figures = () =>
        {
            result.ShouldMatch("Rework Figures");
        };
    }
}