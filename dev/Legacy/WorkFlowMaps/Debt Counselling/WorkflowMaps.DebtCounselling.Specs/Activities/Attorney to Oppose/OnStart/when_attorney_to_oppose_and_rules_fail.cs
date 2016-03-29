using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Attorney_to_Oppose.OnStart
{
    [Subject("Activity => Attorney_To_Oppose => OnStart")]
    internal class when_attorney_to_oppose_and_rules_fail : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = true;
            workflowData.DebtCounsellingKey = 1;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckAttorneyToOpposeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Attorney_to_Oppose(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}