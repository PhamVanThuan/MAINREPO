using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Create_Personal_Loan_Lead.OnStart
{
    [Subject("State => Create_Personal_Loan_Lead => OnStart")]
    internal class when_create_personal_loan_lead : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Manage_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}