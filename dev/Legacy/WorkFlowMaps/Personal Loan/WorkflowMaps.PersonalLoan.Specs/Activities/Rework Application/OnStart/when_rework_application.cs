using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rework_Application.OnStart
{
    [Subject("Activity => Rework_Application => OnStart")]
    internal class when_rework_application : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rework_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}