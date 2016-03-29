using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Termination_Letter.OnStart
{
    [Subject("Activity => Send_Termination_Letter => OnStart")]
    internal class when_cannot_send_termination_letter : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;
        private static string message;

        private Establish context = () =>
        {
            result = true;
            client = An<IDebtCounselling>();
            client.WhenToldTo(x => x.CheckSendTerminationLetterRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(false);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Send_Termination_Letter(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}