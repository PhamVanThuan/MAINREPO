using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._45_Days.OnStart
{
    [Subject("Activity => 45_Days => OnStart")]
    internal class when_45_days_and_timer_has_not_previously_fired : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.HasInstancePerformedActivity((IDomainMessageCollection)messages, instanceData.ID, "45 Days Cancel")).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_45_Days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}