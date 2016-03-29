using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cash_Payment.OnGetStageTransition
{
    [Subject("Activity => Cash_Payment => OnGetStageTransition")] // AutoGenerated
    internal class when_cash_payment : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Cash_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}