using Machine.Specifications;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Reactivate_NTUd_Policy.OnComplete
{
    [Subject("Activity => Reactivate_NTUd_Policy => OnComplete")]
    internal class when_reactivate_ntud_policy : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;

        private Establish context = () =>
        {
            result = false;
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Reactivate_NTUd_Policy(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}