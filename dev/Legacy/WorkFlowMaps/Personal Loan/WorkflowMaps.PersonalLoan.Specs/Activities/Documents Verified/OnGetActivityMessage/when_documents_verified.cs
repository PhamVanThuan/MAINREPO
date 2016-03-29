using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Documents_Verified.OnGetActivityMessage
{
    [Subject("Activity => Documents_Verified => OnGetActivityMessage")]
    internal class when_documents_verified : WorkflowSpecPersonalLoans
    {
        private static string message;

        private Establish context = () =>
        {
            message = "test";
        };

        private Because of = () =>
        {
            message = workflow.GetActivityMessage_Documents_Verified(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            message.ShouldBeEmpty();
        };
    }
}