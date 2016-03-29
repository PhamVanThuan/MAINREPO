using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.States.Manage_Application.OnEnter
{
    [Subject("State => Manage_Application => OnEnter")]
    internal class when_manage_application : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Manage_Application(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}