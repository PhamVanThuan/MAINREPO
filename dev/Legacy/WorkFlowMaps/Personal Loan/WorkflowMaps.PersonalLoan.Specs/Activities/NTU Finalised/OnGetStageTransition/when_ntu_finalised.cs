using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU_Finalised.OnGetStageTransition
{
    [Subject("Activity => NTU_Finalised => OnGetStageTransition")]
    internal class when_ntu_finalised : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "1234";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_NTU_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}