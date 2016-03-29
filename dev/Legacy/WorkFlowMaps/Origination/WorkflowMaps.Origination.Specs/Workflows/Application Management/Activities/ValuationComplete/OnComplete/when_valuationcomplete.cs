using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.ValuationComplete.OnComplete
{
    [Subject("Activity => ValuationComplete => OnComplete")]
    internal class when_valuationcomplete : WorkflowSpecApplicationManagement
    {
        private static IApplicationManagement client;
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            client = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_ValuationComplete(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_send_email_to_consultant = () =>
        {
            client.WasToldTo(x => x.SendEmailToConsultantForValuationDone((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}