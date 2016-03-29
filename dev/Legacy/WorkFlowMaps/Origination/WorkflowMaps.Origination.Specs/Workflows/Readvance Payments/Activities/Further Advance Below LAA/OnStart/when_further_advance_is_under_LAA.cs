using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Further_Advance_Below_LAA.OnStart
{
    [Subject("Activity => Further_Advance_Below_LAA => OnStart")]
    internal class when_further_advance_is_under_LAA : WorkflowSpecReadvancePayments
    {
        private static IFL client;
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            client = An<IFL>();
            client.WhenToldTo(x => x.CheckIsFurtherAdvanceBelowLAARules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Advance_Below_LAA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}