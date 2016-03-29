using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Calculate_Application.OnComplete
{
    [Subject("Activity => Calculate_Application => OnComplete")]
    internal class when_calculate_application : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = "Test";
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Calculate_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}