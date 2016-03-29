using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Continue_Litigation.OnComplete
{
    [Subject("Activity => Continue_Litigation => OnComplete")]
    internal class when_reminder_email_sent : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IDebtCounselling client;

        private Establish context = () =>
            {
                result = false;
                client = An<IDebtCounselling>();
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Continue_Litigation(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_send_the_litigation_reminder_email = () =>
            {
                client.WasToldTo(x => x.SendLitigationReminderInternalEmail((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey));
            };
    }
}