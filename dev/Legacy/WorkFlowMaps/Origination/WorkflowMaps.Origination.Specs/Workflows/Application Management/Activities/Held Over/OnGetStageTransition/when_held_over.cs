using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Held_Over.OnGetStageTransition
{
    [Subject("Activity => Held_Over => OnGetStageTransition")]
    internal class when_held_over : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Held_Over(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_held_over = () =>
        {
            result.ShouldMatch("Held Over");
        };
    }
}