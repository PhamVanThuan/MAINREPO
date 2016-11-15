using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Reactivate_NTUd_Policy.OnGetStageTransition
{
    [Subject("Activity => Reactivate_NTUd_Policy => OnGetStageTransition")] // AutoGenerated
    internal class when_reactivate_ntud_policy : WorkflowSpecLife
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reactivate_NTUd_Policy(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}