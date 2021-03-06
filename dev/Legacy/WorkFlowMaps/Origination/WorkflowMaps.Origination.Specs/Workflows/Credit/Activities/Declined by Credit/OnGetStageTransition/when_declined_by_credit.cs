using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.Activities.Declined_by_Credit.OnGetStageTransition
{
    [Subject("Activity => Declined_by_Credit => OnGetStageTransition")] // AutoGenerated
    internal class when_declined_by_credit : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}