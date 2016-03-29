using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Reinstate_NTU.OnGetActivityMessage
{
    [Subject("Activity => Reinstate NTU => OnGetActivityMessage")]
    internal class when_reinstate_ntu : WorkflowSpecPersonalLoans
    {
        private static string result;

        private Establish context = () =>
        {
            result = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reinstate_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeEmpty();
        };
    }
}