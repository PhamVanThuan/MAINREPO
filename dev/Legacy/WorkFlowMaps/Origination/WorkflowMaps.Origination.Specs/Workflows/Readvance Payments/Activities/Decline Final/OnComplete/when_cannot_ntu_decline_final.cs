using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Decline_Final.OnComplete
{
    [Subject("Activity => Decline_Final => OnComplete")]
    internal class when_cannot_ntu_decline_final : WorkflowSpecReadvancePayments
    {
        private static IFL client;
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            client = An<IFL>();
            client.WhenToldTo(x => x.CheckNTUDeclineFinalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline_Final(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}