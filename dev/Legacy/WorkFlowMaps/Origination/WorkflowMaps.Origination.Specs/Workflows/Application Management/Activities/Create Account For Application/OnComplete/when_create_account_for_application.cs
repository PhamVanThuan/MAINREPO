using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Create_Account_For_Application.OnComplete
{
    [Subject("Activity => Create_Account_For_Application => OnComplete")]
    internal class when_create_account_for_application : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;

        private Establish context = () =>
        {
            result = true;
            message = string.Empty;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Account_For_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_create_account_for_application = () =>
        {
            common.WasToldTo(x => x.CreateAccountForApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}