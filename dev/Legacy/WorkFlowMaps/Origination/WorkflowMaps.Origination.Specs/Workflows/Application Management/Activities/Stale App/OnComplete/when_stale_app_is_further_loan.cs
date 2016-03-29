using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Stale_App.OnComplete
{
    [Subject("Activity => Stale_App => OnComplete")]
    internal class when_stale_app_is_further_loan : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IFL furtherLendingClient;
        private static IApplicationManagement applicationManagementClient;
        private static IWorkflowAssignment workflowAssignmentClient;
        private static ICommon commonClient;

        private Establish context = () =>
        {
            result = false;
            message = String.Empty;
            workflowData.IsFL = true;

            //Make fakes
            furtherLendingClient = An<IFL>();
            applicationManagementClient = An<IApplicationManagement>();
            workflowAssignmentClient = An<IWorkflowAssignment>();
            commonClient = An<ICommon>();

            domainServiceLoader.RegisterMockForType<IFL>(furtherLendingClient);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagementClient);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignmentClient);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Stale_App(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_initial_further_lending_ntu = () =>
        {
            furtherLendingClient.WasToldTo(x => x.InitialFLNTU((IDomainMessageCollection)messages, paramsData.ADUserName, workflowData.ApplicationKey, instanceData.ID));
        };
    }
}