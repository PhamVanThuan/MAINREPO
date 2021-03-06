using Machine.Specifications;

namespace WorkflowMaps.ReadvancePayments.Specs.Activities.Suretyship_Signed.OnGetStageTransition
{
    [Subject("Activity => Suretyship_Signed => OnGetStageTransition")] // AutoGenerated
    internal class when_suretyship_signed : WorkflowSpecReadvancePayments
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Suretyship_Signed(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}