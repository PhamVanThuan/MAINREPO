using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.NTU_Finalised.OnStart
{
    [Subject("Activity => NTU_Finalised => OnStart")]
    internal class when_ntu_finalised_and_aduser_is_not_in_same_branch : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            appMan = An<IApplicationManagement>();
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";
            appMan.WhenToldTo(x => x.CheckADUserInSameBranchRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_NTU_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_aduser_in_same_branch_rules = () =>
        {
            appMan.WasToldTo(x => x.CheckADUserInSameBranchRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning, paramsData.ADUserName));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}