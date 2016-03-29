using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Email_Disbursed_Letter.OnGetStageTransition
{
    [Subject("Activity => Email_Disbursed_Letter => OnGetStageTransition")]
    internal class when_email_disbursed_letter : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Email_Disbursed_Letter(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}