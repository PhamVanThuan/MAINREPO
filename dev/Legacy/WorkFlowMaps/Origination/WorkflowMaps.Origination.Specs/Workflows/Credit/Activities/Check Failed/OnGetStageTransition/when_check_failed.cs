using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Check_Failed.OnGetStageTransition
{
    [Subject("Activity => Check_Failed => OnGetStageTransition")] // AutoGenerated
    internal class when_check_failed : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Check_Failed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}