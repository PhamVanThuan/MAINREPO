using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._45_Days_Cancel.OnStart
{
    [Subject("Activity => 45_Days_Cancel => OnStart")]
    internal class when_45_days_cancel : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static ICommon common;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = true;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<string>())).Return(false);

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckFortyFiveDayReminderRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_45_Days_Cancel(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeTrue();
        };
    }
}