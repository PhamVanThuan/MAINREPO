using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_to_Litigation.OnComplete
{
    [Subject("Activity => Send_to_Litigation => OnComplete")]
    internal class when_cannot_send__to_litigation : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;
        private static string message;

        private Establish context = () =>
            {
                workflowData.SentToLitigation = true;
                result = true;
                client = An<IDebtCounselling>();
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                client.WhenToldTo(x => x.CheckSendToLitigationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                    Param.IsAny<bool>())).Return(false);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Send_to_Litigation(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };

        private It should_set_the_SentToLitigation_data_property_to_false = () =>
            {
                workflowData.SentToLitigation.ShouldBeFalse();
            };
    }
}