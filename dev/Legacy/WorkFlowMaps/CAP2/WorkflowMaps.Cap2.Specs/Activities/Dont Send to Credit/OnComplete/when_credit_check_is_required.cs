using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Cap2;

namespace WorkflowMaps.Cap2.Specs.Activities.Dont_Send_to_Credit.OnComplete
{
    [Subject("Activity => Dont_Send_to_Credit => OnStart")]
    internal class when_credit_check_is_required : WorkflowSpecCap2
    {
        private static bool result;
        private static ICap2 client;

        private Establish context = () =>
        {
            client = An<ICap2>();
            result = false;
            client.WhenToldTo(x => x.IsCreditCheckRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
            domainServiceLoader.RegisterMockForType<ICap2>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Dont_Send_to_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}