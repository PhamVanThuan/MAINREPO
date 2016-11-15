﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Prepare_For_Credit.OnEnter
{
    [Subject("State => Prepare_For_Credit => OnEnter")]
    internal class when_entering_prepare_for_credit : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Prepare_For_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}