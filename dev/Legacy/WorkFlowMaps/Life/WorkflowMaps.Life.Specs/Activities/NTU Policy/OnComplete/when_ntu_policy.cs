using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.NTU_Policy.OnComplete
{
    [Subject("Activity => NTU_Policy => OnComplete")]
    internal class when_ntu_policy : WorkflowSpecLife
    {
        private static bool result;
        private static string message;
        private static ILife client;

        private Establish context = () =>
        {
            result = false;
            ((ParamsDataStub)paramsData).StateName = "test";
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_NTU_Policy(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_last_ntu_state_property = () =>
        {
            workflowData.LastNTUState.ShouldMatch(paramsData.StateName);
        };

        private It should_ntu_policy = () =>
        {
            client.WasToldTo(x => x.NTUPolicy(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
        };
    }
}