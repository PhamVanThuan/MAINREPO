using Machine.Specifications;

namespace WorkflowMaps.Life.Specs.States.NTU_Reactivated.OnAutoForward
{
    [Subject("State => NTU_Reactivated => OnAutoForward")]
    internal class when_ntu_reactivated : WorkflowSpecLife
    {
        private static string fwdState;

        private Establish context = () =>
        {
            workflowData.LastNTUState = "Quote";
        };

        private Because of = () =>
        {
            fwdState = workflow.GetForwardStateName_NTU_Reactivated(instanceData, workflowData, paramsData, messages);
        };

        private It should_forward_to_the_state_prior_to_the_NTU = () =>
        {
            fwdState.ShouldBeTheSameAs(workflowData.LastNTUState);
        };
    }
}