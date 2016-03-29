using Machine.Specifications;
using System;

namespace WorkflowMaps.DebtCounselling.Specs.States.Return_to_State.OnAutoForward
{
    [Subject("State => Return_to_State => OnAutoForward")]
    internal class when_return_to_state : WorkflowSpecDebtCounselling
    {
        private static string result;
        private static string expectedState;

        private Establish context = () =>
        {
            result = String.Empty;
            expectedState = "Manage Proposal";
            workflowData.PreviousState = expectedState;
        };

        private Because of = () =>
        {
            result = workflow.GetForwardStateName_Return_to_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_previous_debt_counselling_state = () =>
        {
            result.ShouldEqual<string>(expectedState);
        };
    }
}