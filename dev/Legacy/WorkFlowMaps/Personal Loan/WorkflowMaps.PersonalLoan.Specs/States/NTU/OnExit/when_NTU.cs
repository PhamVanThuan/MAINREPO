using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.States.NTU.OnExit
{
    [Subject("State => NTU => OnExit")]
    internal class when_NTU : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}