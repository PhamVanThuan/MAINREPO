using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Application_in_Order.OnStart
{
    [Subject("Activity => Application_in_Order => OnStart")]
    internal class when_application_in_order : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Application_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}