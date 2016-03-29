using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.States.Manage_Application.OnExit
{
    [Subject("State => Manage_Application => OnExit")]
    internal class when_manage_application : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.PreviousState = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Manage_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}