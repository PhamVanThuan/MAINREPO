using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Common_Circumstance.OnEnter
{
    [Subject("State => Common_Circumstance => OnEnter")] // AutoGenerated
    internal class when_common_circumstance : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Circumstance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}