using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Disbursed_Timer.OnComplete
{
    [Subject("Activity => Disbursed_Timer => OnComplete")]
    internal class when_disbursed_timer_fires : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static string message;
        private static ICommon commonClient;
        private static IPersonalLoan client;

        private Establish context = () =>
            {
                result = false;
                commonClient = An<ICommon>();
                client = An<IPersonalLoan>();
                domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Disbursed_Timer(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_send_the_client_an_sms = () =>
            {
                client.WasToldTo(x => x.SendSMSToApplicantUponDisbursement(Param.IsAny<IDomainMessageCollection>(),
                    Param.IsAny<int>()));
            };

        private It should_send_the_disbursement_letter = () =>
            {
                //still needs to be implemented
            };
    }
}