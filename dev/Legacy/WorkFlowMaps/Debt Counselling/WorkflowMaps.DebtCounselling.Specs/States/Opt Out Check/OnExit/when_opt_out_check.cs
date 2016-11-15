using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Opt_Out_Check.OnExit
{
    [Subject("State => Opt_Out_Check => OnExit")] // AutoGenerated
    internal class when_opt_out_check : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Opt_Out_Check(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}