using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.States.Manage_Lead.OnExit
{
    [Subject("State => Manage_Lead => OnExit")]
    internal class when_manage_lead : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Manage_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}