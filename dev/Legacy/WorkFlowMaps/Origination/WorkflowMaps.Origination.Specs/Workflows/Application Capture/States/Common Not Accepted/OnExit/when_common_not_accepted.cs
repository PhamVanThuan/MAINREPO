using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.States.Common_Not_Accepted.OnExit
{
    [Subject("State => Common_Not_Accepted => OnExit")]
    internal class when_common_not_accepted : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = "test";
            workflowData.Last_NTU_State = "test";
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_last_state_property = () =>
        {
            workflowData.Last_State.ShouldBeTheSameAs(paramsData.StateName);
        };

        private It should_set_last_ntu_state_property = () =>
        {
            workflowData.Last_NTU_State.ShouldBeTheSameAs(paramsData.StateName);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}