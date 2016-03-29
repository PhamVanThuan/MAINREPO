using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.IsResub.OnGetStageTransition
{
    [Subject("Activity => IsResub => OnGetStageTransition")] // AutoGenerated
    internal class when_isresub : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_IsResub(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}