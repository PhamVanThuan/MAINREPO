using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Surety_Check_for_FA.OnStart
{
    [Subject("Activity => Surety_Check_for_FA => OnStart")]
    internal class when_surety_check_for_fa : WorkflowSpecReadvancePayments
    {
        private static bool result;

        private Establish because = () =>
        {
            result = false;
            IFL fl = An<IFL>();
            workflowData.ApplicationKey = 1;
            fl.WhenToldTo(x => x.CheckSuretyForReAdvanceRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, false)).Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Surety_Check_for_FA(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}