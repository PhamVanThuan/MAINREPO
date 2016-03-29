using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.NTU.OnStart
{
    [Subject("State => NTU => OnStart")]
    internal class when_NTU : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}