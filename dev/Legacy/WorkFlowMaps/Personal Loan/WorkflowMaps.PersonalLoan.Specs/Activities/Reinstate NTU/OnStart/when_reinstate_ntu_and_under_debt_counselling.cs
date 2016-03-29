using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Reinstate_NTU.OnStart
{
    [Subject("Activity => Reinstate NTU => OnStart")]
    internal class when_reinstate_ntu_and_under_debt_counselling : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;

        private Establish context = () =>
        {
            result = false;
            client = An<IPersonalLoan>();
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
            client.WhenToldTo(x => x.CheckUnderDebtCounsellingRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<bool>(), Param.IsAny<int>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}