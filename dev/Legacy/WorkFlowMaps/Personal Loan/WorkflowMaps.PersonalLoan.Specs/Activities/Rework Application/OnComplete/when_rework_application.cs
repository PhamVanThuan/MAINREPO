using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rework_Application.OnComplete
{
    [Subject("Activity => Rework_Application => OnComplete")]
    internal class when_rework_application : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            var message = String.Empty;
            result = workflow.OnCompleteActivity_Rework_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}