using Machine.Specifications;
using WorkflowMaps.PersonalLoan.Specs;

namespace WorkflowMaps.PersonalLoans.Specs.States.Legal_Agreements.OnExit
{
    [Subject("State => Legal_Agreements => OnExit")] // AutoGenerated
    internal class when_legal_agreements : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Legal_Agreements(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}