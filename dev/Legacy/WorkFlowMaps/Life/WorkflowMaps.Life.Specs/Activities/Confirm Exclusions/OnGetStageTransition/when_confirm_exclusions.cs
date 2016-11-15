using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Confirm_Exclusions.OnGetStageTransition
{
    [Subject("Activity => Confirm_Exclusions => OnGetStageTransition")] // AutoGenerated
    internal class when_confirm_exclusions : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Confirm_Exclusions(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}