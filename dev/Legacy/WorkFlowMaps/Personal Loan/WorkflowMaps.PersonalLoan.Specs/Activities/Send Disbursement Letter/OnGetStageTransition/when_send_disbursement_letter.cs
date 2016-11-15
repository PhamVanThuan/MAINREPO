using Machine.Specifications;
using WorkflowMaps.PersonalLoan.Specs;

namespace WorkflowMaps.PersonalLoans.Specs.Activities.Send_Disbursement_Letter.OnGetStageTransition
{
    [Subject("Activity => Send_Disbursement_Letter => OnGetStageTransition")] // AutoGenerated
    internal class when_send_disbursement_letter : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Send_Disbursement_Letter(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}