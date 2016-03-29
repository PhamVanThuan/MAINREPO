using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Core.X2;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Life.Specs.Activities.Clear_Cache.OnComplete
{
    [Subject("Activity => Clear_Cache => OnComplete")]
    internal class when_clear_cache : WorkflowSpecLife
    {
        private static ICommon commonClient;
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            commonClient = An<ICommon>();
            paramsData = An<IX2Params>();
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            result = false;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Clear_Cache(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_clear_cache = () =>
        {
            commonClient.WasToldTo(x => x.ClearCache((IDomainMessageCollection)messages, paramsData.Data));
        };

        private It should_return_true_after_calling_clear_cache = () =>
        {
            result.ShouldBeTrue();
        };
    }
}
