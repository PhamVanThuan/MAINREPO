using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.Activities.Create_Instance_Ext.OnGetStageTransition
{
    [Subject("Activity => Create_Instance_Ext => OnGetStageTransition")]
    internal class when_create_instance_ext : WorkflowSpecLife
    {
        private static string result;
        private static string expectedResult;

        private Establish context = () =>
        {
            workflowData.LoanNumber = 1;
            expectedResult = "New automatic Lead created from loan 1";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Instance_Ext(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_new_automatic_lead_created_from_loan_loan_number = () =>
        {
            result.ShouldMatch(expectedResult);
        };
    }
}