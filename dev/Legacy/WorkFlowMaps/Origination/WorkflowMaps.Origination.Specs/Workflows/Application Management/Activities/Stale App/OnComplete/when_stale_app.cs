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
    internal class when_stale_app : WorkflowSpecApplicationManagement
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
            workflowData.IsFL = false;
            workflowData.EWorkFolderID = String.Empty;

            //Make fakes
            furtherLendingClient = An<IFL>();
            applicationManagementClient = An<IApplicationManagement>();
            workflowAssignmentClient = An<IWorkflowAssignment>();
            commonClient = An<ICommon>();

            domainServiceLoader.RegisterMockForType<IFL>(furtherLendingClient);
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(applicationManagementClient);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignmentClient);
            domainServiceLoader.RegisterMockForType<ICommon>(commonClient);

            applicationManagementClient.WhenToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages,
                workflowData.ApplicationKey, instanceData.ID, 4));
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Stale_App(instanceData, workflowData, paramsData, messages, ref message);
        };

        //At the time this test was created "-1" meant don't set offerinformation
        private It should_update_offer_status_to_ntu_with_no_change_to_offer_information = () =>
        {
            commonClient.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 4, -1));
        };

        private It should_send_email_to_consultant_with_ntu_reasons_only = () =>
        {
            applicationManagementClient.WasToldTo(x => x.SendEmailToConsultantForQuery((IDomainMessageCollection)messages, workflowData.ApplicationKey,
                                                                   instanceData.ID, 4));
        };

        private It should_ntu_case = () =>
        {
            applicationManagementClient.WasToldTo(x => x.NTUCase((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}