﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Check_Cash_Payment.OnExit
{
    [Subject("State => Check_Cash_Payment => OnExit")]
    internal class when_exiting_check_cash_payment : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Check_Cash_Payment(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}