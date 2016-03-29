using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Documents.OnStart
{
    [Subject("Activity => Send_Documents => OnStart")]
    internal class when_send_documents : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            var message = String.Empty;
            result = workflow.OnStartActivity_Send_Documents(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}