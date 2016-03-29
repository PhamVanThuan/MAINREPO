using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Termination_Archive.OnReturn
{
    [Subject("State => Termination_Archive => OnReturn")]
    public class when_termination_is_archived : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Termination_Archive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}