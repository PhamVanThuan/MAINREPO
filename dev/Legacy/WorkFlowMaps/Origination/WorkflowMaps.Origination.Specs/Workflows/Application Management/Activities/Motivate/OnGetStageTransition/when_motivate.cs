using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Motivate.OnGetStageTransition
{
    [Subject("Activity => Motivate => OnGetStageTransition")]
    internal class when_motivate : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Motivate(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_motivate = () =>
        {
            result.ShouldBeTheSameAs("Motivate");
        };
    }
}