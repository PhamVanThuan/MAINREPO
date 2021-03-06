﻿using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Escalate_to_Exceptions_Manager.OnStart
{
    [Subject("Activity => Escalate_to_Exceptions_Manager => OnStart")]
    internal class when_escalate_to_exceptions_manager : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Escalate_to_Exceptions_Manager(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}