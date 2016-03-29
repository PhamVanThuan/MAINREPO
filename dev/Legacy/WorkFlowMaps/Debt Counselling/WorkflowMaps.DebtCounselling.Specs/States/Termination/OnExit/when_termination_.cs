using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Termination_.OnExit
{
    [Subject("State => Termination_ => OnExit")] // AutoGenerated
    internal class when_termination_ : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Termination_(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}