using Machine.Specifications;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Credit.Specs.Activities.Declined_by_Credit.OnComplete
{
    [Subject("Activity => Declined_by_Credit => OnComplete")] // AutoGenerated
    internal class when_declined_by_credit : WorkflowSpecCredit
    {
        private static bool result;
        private static ICommon common;
        

        private Establish context = () =>
        {
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Declined_by_Credit(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}