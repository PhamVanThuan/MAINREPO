using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Calculate_Application.OnStart
{
    [Subject("Activity => Calculate_Application => OnStart")]
    internal class when_calculate_application : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Calculate_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}