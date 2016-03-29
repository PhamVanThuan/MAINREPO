using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Failed_To_Opt_Out.OnComplete
{
    [Subject("Activity => Failed_To_Opt_Out => OnComplete")]
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
            result = workflow.OnCompleteActivity_Failed_To_Opt_Out(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}