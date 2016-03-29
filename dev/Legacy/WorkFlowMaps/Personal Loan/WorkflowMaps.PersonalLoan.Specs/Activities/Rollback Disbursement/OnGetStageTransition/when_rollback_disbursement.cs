using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rollback_Disbursement.OnGetStageTransition
{
    [Subject("Activity => Rollback_Disbursement => OnGetStageTransition")]
    internal class when_rollback_disbursement : WorkflowSpecPersonalLoans
    {
        private static string message;

        private Establish context = () =>
        {
            message = "1234";
        };

        private Because of = () =>
        {
            message = workflow.GetStageTransition_Disburse_Funds(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            message.ShouldBeEmpty();
        };
    }
}