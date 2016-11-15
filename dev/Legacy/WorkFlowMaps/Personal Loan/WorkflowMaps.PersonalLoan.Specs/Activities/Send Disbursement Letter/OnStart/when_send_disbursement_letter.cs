using Machine.Specifications;
using WorkflowMaps.PersonalLoan.Specs;

namespace WorkflowMaps.PersonalLoans.Specs.Activities.Send_Disbursement_Letter.OnStart
{
    [Subject("Activity => Send_Disbursement_Letter => OnStart")] // AutoGenerated
    internal class when_send_disbursement_letter : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Send_Disbursement_Letter(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}