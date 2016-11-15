using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Completed_Expired.OnGetStageTransition
{
    [Subject("Activity => Completed_Expired => OnGetStageTransition")] // AutoGenerated
    internal class when_completed_expired : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Completed_Expired(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}