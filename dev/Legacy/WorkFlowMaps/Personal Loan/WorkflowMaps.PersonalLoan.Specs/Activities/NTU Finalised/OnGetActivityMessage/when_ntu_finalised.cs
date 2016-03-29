using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU_Finalised.OnGetActivityMessage
{
    [Subject("Activity => NTU_Finalised => OnGetActivityMessage")]
    internal class when_ntu_finalised : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "1234";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_NTU_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}