using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Documents.OnGetActivityMessage
{
    [Subject("Activity => Send_Documents => OnGetActivityMessage")]
    internal class when_send_documents : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Send_Documents(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}