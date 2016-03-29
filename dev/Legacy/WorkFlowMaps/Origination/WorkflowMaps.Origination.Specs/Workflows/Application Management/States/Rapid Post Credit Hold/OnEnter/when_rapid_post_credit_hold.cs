using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Rapid_Post_Credit_Hold.OnEnter
{
    [Subject("State => Rapid_Post_Credit_Hold => OnEnter")] // AutoGenerated
    internal class when_rapid_post_credit_hold : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Rapid_Post_Credit_Hold(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}