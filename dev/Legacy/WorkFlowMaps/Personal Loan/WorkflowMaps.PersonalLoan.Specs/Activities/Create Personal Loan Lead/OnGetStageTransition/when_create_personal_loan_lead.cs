using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Create_Personal_Loan_Lead.OnGetStageTransition
{
    [Subject("State => Create_Personal_Loan_Lead => OnGetStageTransition")]
    internal class when_create_personal_loan_lead : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Personal_Loan_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldEqual<string>(String.Empty);
        };
    }
}