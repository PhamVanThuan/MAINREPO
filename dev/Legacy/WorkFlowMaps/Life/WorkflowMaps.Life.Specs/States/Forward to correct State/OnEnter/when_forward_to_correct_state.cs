using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.States.Forward_to_correct_State.OnEnter
{
    [Subject("State => Forward_To_Correct_State => OnEnter")]
    internal class when_forward_to_correct_state : WorkflowSpecLife
    {
        private static bool result;
        private static ILife client;
        private static int policyTypeKey;

        private Establish context = () =>
        {
            result = false;
            policyTypeKey = 1;
            client = An<ILife>();
            client.WhenToldTo(x => x.GetPolicyTypeKeyForOfferLife(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(policyTypeKey);
            domainServiceLoader.RegisterMockForType<ILife>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Forward_to_correct_State(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_set_the_policy_type_key_data_property = () =>
        {
            workflowData.PolicyTypeKey.ShouldEqual(policyTypeKey);
        };
    }
}