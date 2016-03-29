using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Client_Refuse.OnStart
{
    [Subject("Activity => Client_Refuse => OnStart")]
    internal class when_client_refuse_and_aduser_in_same_branch : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckADUserInSameBranchRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Client_Refuse(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_aduser_in_same_branch = () =>
        {
            appMan.WasToldTo(x => x.CheckADUserInSameBranchRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning, paramsData.ADUserName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}