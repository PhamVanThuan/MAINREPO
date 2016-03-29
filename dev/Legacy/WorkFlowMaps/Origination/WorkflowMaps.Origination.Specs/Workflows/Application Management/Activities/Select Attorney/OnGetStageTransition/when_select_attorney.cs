using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Select_Attorney.OnGetStageTransition
{
    [Subject("Activity => Select_Attorney => OnGetStageTransition")]
    internal class when_select_attorney : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Select_Attorney(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_select_attorney_stagetransition = () =>
        {
            result.ShouldBeTheSameAs("Select Attorney");
        };
    }
}