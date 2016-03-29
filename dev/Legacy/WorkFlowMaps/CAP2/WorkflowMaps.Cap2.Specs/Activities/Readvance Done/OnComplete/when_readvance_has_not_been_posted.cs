using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.Activities.Readvance_Done.OnComplete
{
    [Subject("Activity => Readvance_Done => OnComplete")]
    internal class when_readvance_has_not_been_posted : WorkflowSpecCap2
    {
        private static ICap2 client;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            client = An<ICap2>();
            result = false;
            message = string.Empty;
            client.WhenToldTo(x => x.CheckReadvanceDoneRules(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<bool>(), Param.IsAny<int>())).Return(false);
            domainServiceLoader.RegisterMockForType<ICap2>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Readvance_Done(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}