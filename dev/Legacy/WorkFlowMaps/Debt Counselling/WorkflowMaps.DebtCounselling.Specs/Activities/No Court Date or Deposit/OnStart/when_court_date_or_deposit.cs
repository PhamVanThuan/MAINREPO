using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.No_Court_Date_or_Deposit.OnStart
{
    [Subject("Activity => No_Court_Date_or_Deposit => OnStart")]
    internal class when_court_date_or_deposit : WorkflowSpecDebtCounselling
    {
        private static IDebtCounselling client;
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckNoDateNoPayRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_No_Court_Date_or_Deposit(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}