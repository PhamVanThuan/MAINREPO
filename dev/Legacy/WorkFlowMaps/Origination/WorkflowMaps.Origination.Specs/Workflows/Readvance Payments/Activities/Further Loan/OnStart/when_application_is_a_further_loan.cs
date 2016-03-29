using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Further_Loan.OnStart
{
    [Subject("Activity => Further_Loan => OnStart")]
    internal class when_application_is_a_further_loan : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL client;

        private Establish context = () =>
        {
            result = false;
            client = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(client);
            client.WhenToldTo(x => x.IsFurtherLoanApplication(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Loan(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}