using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.EXT_NTU.OnStart
{
    [Subject("Activity => EXT_NTU => OnStart")]
    internal class when_ext_ntu : WorkflowSpecPersonalLoans
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_EXT_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}