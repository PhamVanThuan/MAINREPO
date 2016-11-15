using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Bond_Exclusions_Arrears.OnExit
{
    [Subject("State => Bond_Exclusions_Arrears => OnExit")] // AutoGenerated
    internal class when_bond_exclusions_arrears : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Bond_Exclusions_Arrears(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}