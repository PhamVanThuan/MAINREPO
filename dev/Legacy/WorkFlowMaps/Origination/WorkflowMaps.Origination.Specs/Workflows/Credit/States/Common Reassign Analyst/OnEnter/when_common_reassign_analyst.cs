using Machine.Specifications;

namespace WorkflowMaps.Credit.Specs.States.Common_Reassign_Analyst.OnEnter
{
    [Subject("State => Common_Reassign_Analyst => OnEnter")] // AutoGenerated
    internal class when_common_reassign_analyst : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Common_Reassign_Analyst(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}