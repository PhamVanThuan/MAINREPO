using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Common_Reassign_Analyst.OnExit
{
    [Subject("State => Common_Reassign_Analyst => OnExit")] // AutoGenerated
    internal class when_common_reassign_analyst : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Reassign_Analyst(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}