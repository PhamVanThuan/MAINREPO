using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Failed_To_Opt_Out.OnStart
{
    [Subject("Activity => Failed_To_Opt_Out => OnStart")]
    internal class when_failed_to_opt_out : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Failed_To_Opt_Out(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}