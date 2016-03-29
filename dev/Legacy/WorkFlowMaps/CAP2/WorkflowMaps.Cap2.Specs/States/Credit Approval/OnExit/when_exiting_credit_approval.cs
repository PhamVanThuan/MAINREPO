﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Credit_Approval.OnExit
{
    [Subject("State => Credit_Approval => OnExit")]
    internal class when_exiting_credit_approval : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Credit_Approval(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}