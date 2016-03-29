using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Require_Opt_Out_SuperLo.OnStart
{
    [Subject("State => Require_Opt_Out_SuperLo => OnStart")]
    internal class when_opt_out_super_lo_rules_fail : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL fl;

        private Establish context = () =>
        {
            result = false;
            fl = An<IFL>();
            fl.WhenToldTo(x => x.CheckSuperLoOptOutRequiredRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, false)).Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Require_Opt_Out_SuperLo(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}