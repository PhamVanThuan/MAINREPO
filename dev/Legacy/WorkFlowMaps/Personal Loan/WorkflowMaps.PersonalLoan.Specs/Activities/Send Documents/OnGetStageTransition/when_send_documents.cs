using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Documents.OnGetStageTransition
{
    [Subject("Activity => Send_Documents => OnGetStageTransition")]
    internal class when_send_documents : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_Documents(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}