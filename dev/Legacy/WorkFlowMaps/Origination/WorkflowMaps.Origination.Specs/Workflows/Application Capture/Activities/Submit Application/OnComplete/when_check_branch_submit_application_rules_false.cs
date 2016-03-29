using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Submit_Application.OnComplete
{
    [Subject("Activity => Submit_Application => OnComplete")]
    internal class when_check_branch_submit_application_rules_false : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static IApplicationCapture client;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;
            client = An<IApplicationCapture>();
            client.WhenToldTo(x => x.CheckBranchSubmitApplicationRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(client);
            var common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Submit_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}