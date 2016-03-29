using Machine.Fakes;

using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.App_Not_Accepted_Finalised.OnStart
{
    [Subject("Activity => App_Not_Accepted_Finalised => OnStart")]
    internal class when_app_not_accepted_finalised : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static bool clientResult;
        private static IApplicationManagement client;

        private Establish context = () =>
        {
            //just so that they are not the same
            result = true;
            clientResult = false;

            //Mocking
            client = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_App_Not_Accepted_Finalised(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_aduser_is_in_same_branch = () =>
        {
            client.WasToldTo(x => x.CheckADUserInSameBranchRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>(), Param.IsAny<string>()));
        };

        private It should_return_what_client_check_aduser_same_branch_rule_returns = () =>
        {
            result.ShouldEqual<bool>(clientResult);
        };
    }
}