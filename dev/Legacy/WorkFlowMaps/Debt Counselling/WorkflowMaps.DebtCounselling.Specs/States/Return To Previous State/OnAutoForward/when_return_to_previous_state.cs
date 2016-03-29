using Machine.Specifications;
using System;

namespace WorkflowMaps.DebtCounselling.Specs.States.Return_To_Previous_State.OnAutoForward
{
    [Subject("State => Return_to_State => OnAutoForward")]
    internal class when_return_to_previous_state : WorkflowSpecDebtCounselling
    {
        private static string result;
        private static string expectedState;

        private Establish context = () =>
        {
            result = String.Empty;
            expectedState = "Pend Proposal";
            workflowData.PreviousState = expectedState;
        };

        private Because of = () =>
        {
            result = workflow.GetForwardStateName_Return_To_Previous_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_previous_debt_counselling_state = () =>
        {
            result.ShouldEqual<string>(expectedState);
        };
    }
}