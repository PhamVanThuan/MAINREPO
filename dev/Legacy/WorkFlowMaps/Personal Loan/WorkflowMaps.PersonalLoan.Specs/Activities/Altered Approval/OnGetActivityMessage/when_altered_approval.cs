﻿using Machine.Specifications;
using WorkflowMaps.PersonalLoan.Specs;

namespace WorkflowMaps.PersonalLoans.Specs.Activities.Altered_Approval.OnGetActivityMessage
{
    [Subject("Activity => Altered_Approval => OnGetActivityMessage")] // AutoGenerated
    internal class when_altered_approval : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Altered_Approval(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}