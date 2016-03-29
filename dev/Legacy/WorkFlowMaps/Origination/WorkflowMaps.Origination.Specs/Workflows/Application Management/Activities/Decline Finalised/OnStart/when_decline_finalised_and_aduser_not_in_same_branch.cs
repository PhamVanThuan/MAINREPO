using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Decline_Finalised.OnStart
{
    [Subject("Activity => Decline_Finalised => OnStart")]
    internal class when_decline_finalised_and_aduser_not_in_same_branch : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = true;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckADUserInSameBranchRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}