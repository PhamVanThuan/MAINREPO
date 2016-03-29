using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Further_Advance.OnStart
{
    [Subject("Activity => Further_Advance => OnComplete")]
    internal class when_application_is_not_a_further_advance : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL client;

        private Establish context = () =>
        {
            result = true;
            client = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(client);
            client.WhenToldTo(x => x.IsFurtherAdvanceApplication(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Advance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}