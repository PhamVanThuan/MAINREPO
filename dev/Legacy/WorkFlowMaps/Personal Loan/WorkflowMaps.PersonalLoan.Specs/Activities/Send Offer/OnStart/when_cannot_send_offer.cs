using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Send_Offer.OnStart
{
    [Subject("Activity => Send_Offer => OnStart")]
    internal class when_cannot_send_offer : WorkflowSpecPersonalLoans
    {
        [Subject("Activity => Send_Offer => OnStart")]
        internal class when_can_send_offer : WorkflowSpecPersonalLoans
        {
            private static bool result;
            private static IPersonalLoan personalLoan;

            private Establish context = () =>
            {
                result = true;
                personalLoan = An<IPersonalLoan>();
                domainServiceLoader.RegisterMockForType<IPersonalLoan>(personalLoan);
                personalLoan.WhenToldTo(x => x.CheckSendOfferRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
            };

            private Because of = () =>
            {
                result = workflow.OnStartActivity_Send_Offer(instanceData, workflowData, paramsData, messages);
            };

            private It should_return_false = () =>
            {
                result.ShouldBeFalse();
            };
        }
    }
}