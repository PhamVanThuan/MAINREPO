using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Application_Received.OnComplete
{
    [Subject("Activity => Application_Received => OnComplete")]
    internal class when_application_recieved : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IFL client;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            client = An<IFL>();

            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Application_Received(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_add_detail_types = () =>
        {
            client.WasToldTo(x => x.AddDetailTypes((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}