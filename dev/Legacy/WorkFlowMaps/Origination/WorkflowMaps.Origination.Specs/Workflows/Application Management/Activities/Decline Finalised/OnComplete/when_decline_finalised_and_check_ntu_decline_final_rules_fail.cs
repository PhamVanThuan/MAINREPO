using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline_Finalised.OnComplete
{
    [Subject("Activity => Decline_Finalised => OnComplete")]
    internal class when_decline_finalised_and_check_ntu_decline_final_rules_fail : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IFL fl;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            fl = An<IFL>();
            fl.WhenToldTo(x => x.CheckNTUDeclineFinalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline_Finalised(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldEqual(false);
        };
    }
}