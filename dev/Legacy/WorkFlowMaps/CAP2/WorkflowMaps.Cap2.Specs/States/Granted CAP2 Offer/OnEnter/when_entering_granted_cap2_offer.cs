﻿using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.Granted_CAP2_Offer.OnEnter
{
    [Subject("State => Granted_CAP2_Offer => OnEnter")]
    internal class when_entering_granted_cap2_offer : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Granted_CAP2_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}