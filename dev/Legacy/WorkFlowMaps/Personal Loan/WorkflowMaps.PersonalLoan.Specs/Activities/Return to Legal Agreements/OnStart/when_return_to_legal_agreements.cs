using Machine.Specifications;
using System;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Return_to_Legal_Agreements.OnStart
{
    [Subject("Activity => Return_to_Legal_Agreements => OnStart")]
    internal class when_return_to_legal_agreements : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            var message = String.Empty;
            result = workflow.OnStartActivity_Return_to_Legal_Agreements(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}