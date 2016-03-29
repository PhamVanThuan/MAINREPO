using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Return_to_Legal_Agreements.OnGetActivityMessage
{
    [Subject("Activity => Return_to_Legal_Agreements => OnGetActivityMessage")]
    internal class when_return_to_legal_agreements : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Return_to_Legal_Agreements(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}