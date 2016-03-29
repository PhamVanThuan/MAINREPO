using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rollback_Disbursement.OnGetActivityMessage
{
    [Subject("Activity => Rollback_Disbursement => OnGetActivityMessage")]
    internal class when_rollback_disbursement : WorkflowSpecPersonalLoans
    {
        private static string message;

        private Establish context = () =>
        {
            message = "12345";
        };

        private Because of = () =>
        {
            message = workflow.GetActivityMessage_Rollback_Disbursement(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            message.ShouldBeEmpty();
        };
    }
}