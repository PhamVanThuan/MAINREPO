using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.States.Manage_Lead.OnEnter
{
    [Subject("State => Manage_Lead => OnEnter")]
    internal class when_manage_lead : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Manage_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}