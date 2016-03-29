using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._45_Days_Cancel.OnStart
{
    [Subject("Activity => 45_Days_Cancel => OnStart")]
    internal class when_45_days_cancel_and_45_days_lapsed_activity_previously_performed : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string activity;
        private static ICommon common;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;
            activity = "45 Days Lapsed";
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.DebtCounsellingKey = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<string>())).Return(true);

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_45_Days_Cancel(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_45_days_lapsed_activity_has_previously_been_performed = () =>
        {
            common.WasToldTo(x => x.HasInstancePerformedActivity((IDomainMessageCollection)messages, instanceData.ID, activity));
        };

        private It should_not_check_45_day_reminder_rules = () =>
        {
            client.WasNotToldTo(x => x.CheckFortyFiveDayReminderRules((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, paramsData.IgnoreWarning));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}