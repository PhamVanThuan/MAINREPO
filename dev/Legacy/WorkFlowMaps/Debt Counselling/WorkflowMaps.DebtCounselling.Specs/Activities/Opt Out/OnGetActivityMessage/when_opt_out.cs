using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Opt_Out.OnGetActivityMessage
{
    [Subject("Activity => Opt_Out => OnGetActivityMessage")]
    internal class when_opt_out : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Opt_Out(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}