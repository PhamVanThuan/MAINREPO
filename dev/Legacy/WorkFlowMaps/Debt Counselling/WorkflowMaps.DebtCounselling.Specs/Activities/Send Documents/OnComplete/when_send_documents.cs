using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Documents.OnComplete
{
    [Subject("Activity => Send_Documents => OnComplete")] // AutoGenerated
    internal class when_send_documents : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Send_Documents(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}