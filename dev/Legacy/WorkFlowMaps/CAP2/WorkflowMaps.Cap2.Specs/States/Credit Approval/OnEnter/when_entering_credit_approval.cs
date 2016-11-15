﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Credit_Approval.OnEnter
{
    [Subject("State => Credit_Approval => OnEnter")]
    internal class when_entering_credit_approval : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Credit_Approval(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}