using Machine.Specifications;
using WorkflowMaps.PersonalLoan.Specs;

namespace WorkflowMaps.PersonalLoans.Specs.States.Common_Rework.OnExit
{
    [Subject("State => Common_Rework => OnExit")] // AutoGenerated
    internal class when_common_rework : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Rework(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}