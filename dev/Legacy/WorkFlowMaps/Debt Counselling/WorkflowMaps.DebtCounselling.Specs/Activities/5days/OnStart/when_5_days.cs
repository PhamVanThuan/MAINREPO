using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._5days.OnStart
{
    [Subject("Activity => 5_Days => OnStart")]
    internal class when_5_days : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckFiveDaysTerminationReminderRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_5days(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_5_day_termination_reminder_rules = () =>
        {
            client.WasToldTo(x => x.CheckFiveDaysTerminationReminderRules((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, paramsData.IgnoreWarning));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}