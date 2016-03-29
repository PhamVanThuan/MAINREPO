using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Calculate_Application.OnGetActivityMessage
{
    [Subject("Activity => Calculate_Application => OnGetActivityMessage")]
    internal class when_calculate_application : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "Test";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Calculate_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_empty_string = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}