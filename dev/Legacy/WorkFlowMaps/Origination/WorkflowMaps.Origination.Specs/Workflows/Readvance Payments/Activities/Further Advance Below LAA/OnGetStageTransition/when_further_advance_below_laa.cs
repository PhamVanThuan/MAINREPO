using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Further_Advance_Below_LAA.OnGetStageTransition
{
    [Subject("Activity => Further_Advance_Below_LAA => OnGetStageTransition")] // AutoGenerated
    internal class when_further_advance_below_laa : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Further_Advance_Below_LAA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}