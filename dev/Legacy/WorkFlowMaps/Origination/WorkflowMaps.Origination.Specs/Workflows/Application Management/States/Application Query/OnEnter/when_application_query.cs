using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Application_Query.OnEnter
{
    [Subject("State => Application_Query => OnEnter")]
    internal class when_application_query : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static int genericKey;
        private static long instanceID;
        private static int reasonGroupTypeKey;
        private static IApplicationManagement appman;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            appman = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appman);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Application_Query(instanceData, workflowData, paramsData, messages);
        };

        private It should_send_email_to_consultant = () =>
       {
           appman.WasToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages, genericKey, instanceID, 8));
       };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}