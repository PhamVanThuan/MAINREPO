using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Decline_Finalised.OnStart
{
    [Subject("Activity => Decline_Finalised => OnStart")]
    internal class when_decline_finalised_and_aduser_in_same_branch_check_passed : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static bool expectedResult;
        private static string message;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = false;
            expectedResult = true;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((ParamsDataStub)paramsData).ADUserName = "ExpectedADUserName";
            appMan = An<IApplicationManagement>();
            appMan.WhenToldTo(x => x.CheckADUserInSameBranchRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline_Finalised(instanceData, workflowData, paramsData, messages);
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