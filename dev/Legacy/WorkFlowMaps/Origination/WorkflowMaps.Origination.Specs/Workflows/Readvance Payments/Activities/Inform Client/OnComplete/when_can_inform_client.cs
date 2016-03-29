using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Inform_Client.OnComplete
{
    [Subject("Activity => Inform_Client => OnComplete")]
    internal class when_can_inform_client : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL client;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            client = An<IFL>();
            client.WhenToldTo(x => x.CheckInformClientRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Inform_Client(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}