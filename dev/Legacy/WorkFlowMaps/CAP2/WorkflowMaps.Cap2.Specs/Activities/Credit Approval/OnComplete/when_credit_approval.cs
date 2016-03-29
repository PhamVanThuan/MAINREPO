using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Credit_Approval.OnComplete
{
    [Subject("Activity => Credit_Approval => OnComplete")]
    internal class when_credit_approval : WorkflowSpecCap2
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Credit_Approval(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}