using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Create_Instance.OnGetStageTransition
{
    [Subject("Activity => Create_Instance => OnGetStageTransition")]
    internal class when_create_instance : WorkflowSpecLife
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            workflowData.LoanNumber = 1;
            expectedResult = "New manual Lead created from loan 1";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_new_manual_lead_created_from_loan_loan_number = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}