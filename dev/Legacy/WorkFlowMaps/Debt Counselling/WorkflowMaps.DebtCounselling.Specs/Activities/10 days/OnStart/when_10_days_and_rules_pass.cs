using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._10_days.OnStart
{
    [Subject("Activity => 10_Days => OnStart")]
    internal class when_10_days_and_rules_pass : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckTenDaysTerminationReminderRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_10_days(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}