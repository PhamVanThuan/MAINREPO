using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Surety_check_for_Rapid.OnStart
{
    [Subject("State => Surety_Check_For_Rapid => OnStart")]
    internal class when_surety_check_for_rapid_fail_rules : WorkflowSpecReadvancePayments
    {
        private static IFL fl;
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            IFL fl = An<IFL>();
            workflowData.ApplicationKey = 1;
            fl.WhenToldTo(x => x.CheckSuretyForReAdvanceRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, false)).Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Surety_check_for_Rapid(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}